using Berx3.Api.Data;
using Berx3.Api.Models;
using Berx3.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Berx3.Tests.Repositories
{
    public class TShirtRepositoryTests
    {
        private readonly TshirtRepository _repository;
        private readonly AppDbContext _dbContext;

        public TShirtRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Berx3_TestDB")
                .Options;

            _dbContext = new AppDbContext(options);
            _repository = new TshirtRepository(_dbContext);
        }

        [Fact]
        public async Task AddAsync_Should_Add_TShirt()
        {
            // Arrange
            var tshirt = new TShirt { Color = "Red", Size = "M", Quantity = 10 };

            // Act
            await _repository.AddAsync(tshirt);
            var tshirts = await _repository.GetAllAsync();

            // Assert
            Assert.Equal("Red", tshirts.First().Color);
        }

        [Fact]
        public async Task SoftDeleteAsync_Should_Set_IsAvailable_To_False()
        {
            // Arrange
            var tshirt = new TShirt { Color = "Blue", Size = "L", Quantity = 5, IsAvailable = true };
            await _repository.AddAsync(tshirt);

            // Act
            await _repository.SoftDeleteAsync(tshirt.Id);
            var deletedTShirt = await _repository.GetByIdAsync(tshirt.Id);

            // Assert
            Assert.False(deletedTShirt!.IsAvailable);
        }
    }
}
