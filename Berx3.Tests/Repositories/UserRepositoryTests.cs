using Berx3.Api.Data;
using Berx3.Api.Models;
using Berx3.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Berx3.Tests.Repositories
{
    public class UserRepositoryTests
    {
        private readonly UserRepository _repository;
        private readonly AppDbContext _dbContext;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _dbContext = new AppDbContext(options);
            _repository = new UserRepository(_dbContext);
        }

        [Fact]
        public async Task AddAsync_Should_Add_User_To_Database()
        {
            // Arrange
            var user = new User
            {
                Username = "testuser",
                Email = "test@gmail.com",
                PhoneNumber = "123456789"
            };

            // Act
            await _repository.AddAsync(user);
            var users = await _repository.GetAllAsync();

            // Assert
            Assert.Single(users);
            Assert.Equal("testuser", users.First().Username);
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_Only_Active_Users()
        {
            // Arrange
            await _repository.AddAsync(
                new User { 
                    Username = "activeUser",
                    Email = "user1@example.com",
                    PhoneNumber = "12456488",
                    IsActive = true 
                });
            await _repository.AddAsync(
                new User
                {
                    Username = "inactiveUser",
                    Email = "user1@example.com",
                    PhoneNumber = "56865465",
                    IsActive = false
                });

            // Act
            var user = await _repository.GetAllAsync();

            // Assert
            Assert.True(user.First().IsActive);
        }

        [Fact]
        public async Task SoftDeleteAsync_Should_Set_IsActive_False()
        {
            var user = new User
            {
                Username = "DeleteUser",
                Email = "delete@mail.com",
                PhoneNumber = "987564235"
            };

            await _repository.AddAsync(user);

            await _repository.SoftDeleteAsync(user.Id);
            var deletedUser = await _repository.GetByIdAsync(user.Id);

            Assert.False(deletedUser!.IsActive);
        }
    }
}
