using Xunit;
using Microsoft.EntityFrameworkCore;
using SnackCounter.Data;
using SnackCounter.Models.Entities;
using SnackCounter.Models.Enum;
using SnackCounter.Services;
using System;

namespace SnackCounter.Tests
{
    public class DeclineRefillTests
    {
        [Fact]
        public async Task DeclineRefill_ShouldThrowException_WhenWrongEmployeeDeclines()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SnackCounterDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new SnackCounterDbContext(options);

            var refill = new Refill
            {
                Office = OfficeType.Bengaluru,
                Status = RefillStatus.Assigned,
                AssignedEmployeeId = 10
            };

            context.Refills.Add(refill);
            await context.SaveChangesAsync();

            var service = new RefillService(
                context,
                null,
                null,
                null,
                null
            );

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => service.DeclineRefillAsync(refill.Id, 99)
            );
        }
    }
}