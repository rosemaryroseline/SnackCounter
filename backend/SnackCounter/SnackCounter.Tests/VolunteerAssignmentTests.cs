
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
    public class VolunteerAssignmentTests
    {
        [Fact]
        public async Task CreateRefill_ShouldAssignVolunteer_WhenVolunteerAvailable()
        {
            var options = new DbContextOptionsBuilder<SnackCounterDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(w =>
                    w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using var context = new SnackCounterDbContext(options);

            var employee = new Employee
            {
                EmployeeId = "EMP001"
            };

            context.Employees.Add(employee);
            await context.SaveChangesAsync();

            var service = new RefillService(
                context,
                new FakeVolunteerService(employee),
                null!,
                new FakeChatService(),
                null!
            );

            var request = new CreateRefillRequest
            {
                Office = OfficeType.Oslo
            };

            var result = await service.CreateRefillAsync(request);

            Assert.NotNull(result);
            Assert.Equal(employee.Id, result.AssignedEmployeeId);
            Assert.Equal(RefillStatus.Assigned, result.Status);
        }

        private class FakeVolunteerService : IVolunteerService
        {
            private readonly Employee _employee;

            public FakeVolunteerService(Employee employee)
            {
                _employee = employee;
            }

            public Task<Employee?> SelectVolunteerAsync(OfficeType office)
            {
                return Task.FromResult<Employee?>(_employee);
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
