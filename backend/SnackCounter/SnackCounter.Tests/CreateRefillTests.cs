using Xunit;
using Microsoft.EntityFrameworkCore;
using SnackCounter.Data;
using SnackCounter.Models.Entities;
using SnackCounter.Models.DTOs;
using SnackCounter.Models.Enum;
using SnackCounter.Services;
using System;
using System.Threading.Tasks;

namespace SnackCounter.Tests
{
    public class CreateRefillTests
    {
        [Fact]
        public async Task CreateRefill_ShouldThrowException_WhenActiveRefillAlreadyExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SnackCounterDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new SnackCounterDbContext(options);

            context.Refills.Add(new Refill
            {
                Office = OfficeType.Bengaluru,
                Status = RefillStatus.Open
            });

            await context.SaveChangesAsync();

            var service = new RefillService(
                context,
                null,
                null,
                null,
                null
            );

            var request = new CreateRefillRequest
            {
                Office = OfficeType.Bengaluru
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => service.CreateRefillAsync(request)
            );
        }
    }
}