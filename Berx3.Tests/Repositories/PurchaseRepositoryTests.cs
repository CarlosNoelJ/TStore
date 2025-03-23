using Berx3.Api.Data;
using Berx3.Api.Models;
using Berx3.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;


namespace Berx3.Tests.Repositories
{
    public class PurchaseRepositoryTests
    {
        private readonly PurchaseRepository _repository;
        private readonly AppDbContext _dbContext;

        public PurchaseRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Berx3_TestDB")
                .Options;

            _dbContext = new AppDbContext(options);
            _repository = new PurchaseRepository(_dbContext);
        }

        [Fact]
        public async Task AddPurchaseAsync_Should_Create_Purchase_And_Update_Stock()
        {
            // Arrange
            var user = new User { Username = "buyer", Email = "buyer@example.com" };
            var tshirt = new TShirt { Color = "Black", Size = "S", Quantity = 5, IsAvailable = true };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.TShirts.AddAsync(tshirt);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.AddPurchaseAsync(user.Id, tshirt.Id, 2);
            var updatedTShirt = await _dbContext.TShirts.FindAsync(tshirt.Id);

            // Assert
            Assert.Equal("Purchase successful.", result);
            Assert.Equal(3, updatedTShirt!.Quantity);
        }

        [Fact]
        public async Task AddPurchaseAsync_Should_Fail_If_Not_Enough_Stock()
        {
            // Arrange
            var user = new User { Username = "buyer", Email = "buyer@example.com" };
            var tshirt = new TShirt { Color = "white", Size = "XL", Quantity = 1, IsAvailable = true };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.TShirts.AddAsync(tshirt);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.AddPurchaseAsync(user.Id, tshirt.Id, 5);

            // Assert
            Assert.Equal("Not enough stock available.", result);
        }
    }
}
