using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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
    public class CreateRefillSuccessTests
    {
        [Fact]
        public async Task CreateRefill_ShouldCreateRefill_WhenNoActiveRefillExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SnackCounterDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(w =>
                    w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using var context = new SnackCounterDbContext(options);

            var service = new RefillService(
                context,
                new FakeVolunteerService(),
                null!,
                new FakeChatService(),
                null!
            );

            var request = new CreateRefillRequest
            {
                Office = OfficeType.Oslo
            };

            // Act
            var result = await service.CreateRefillAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(OfficeType.Oslo, result.Office);
            Assert.Equal(RefillStatus.Open, result.Status);
            Assert.Null(result.AssignedEmployeeId);
        }

        private class FakeVolunteerService : IVolunteerService
        {
            public Task<Employee?> SelectVolunteerAsync(OfficeType office)
            {
                return Task.FromResult<Employee?>(null);
            }
        }

        private class FakeChatService : IChatService
        {
            public Task SendMessageAsync(string recipient, string message)
            {
                return Task.CompletedTask;
            }
        }
    }
}