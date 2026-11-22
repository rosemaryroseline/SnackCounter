using Xunit;
using Microsoft.EntityFrameworkCore;
using SnackCounter.Data;
using SnackCounter.Models.Entities;
using SnackCounter.Models.Enum;
using SnackCounter.Services;
using System;
using System.Threading.Tasks;

namespace SnackCounter.Tests
{
    public class AcceptRefillTests
    {
        [Fact]
        public async Task AcceptRefill_ShouldThrowException_WhenWrongEmployeeAccepts()
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
                () => service.AcceptRefillAsync(refill.Id, 99)
            );
        }
    }
}