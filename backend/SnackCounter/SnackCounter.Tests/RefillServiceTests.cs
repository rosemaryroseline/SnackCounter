
using Microsoft.EntityFrameworkCore;
using SnackCounter.Data;
using SnackCounter.Interfaces;
using SnackCounter.Models.DTOs;
using SnackCounter.Models.Entities;
using SnackCounter.Models.Enum;
using SnackCounter.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SnackCounter.Tests
{
    public class RefillServiceTests
    {
        [Fact]
        public async Task SubmitPurchase_ShouldThrowException_WhenRefillIsNotAccepted()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SnackCounterDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new SnackCounterDbContext(options);

            var refill = new Refill
            {
                Office = OfficeType.Bengaluru,
                Status = RefillStatus.Assigned
            };

            context.Refills.Add(refill);
            await context.SaveChangesAsync();

            // Act
            var service = new RefillService(
                context,
                null,
                null,
                null,
                null
            );

            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => service.SubmitPurchaseAsync(
                    refill.Id,
                    new SubmitPurchaseRequest
                    {
                        EmployeeId = 1,
                        Amount = 500,
                        Description = "Tea and snacks",
                        ReceiptPath = "receipt.jpg"
                    })
            );
        }
    }
}
