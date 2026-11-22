
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SnackCounter.Data;
using SnackCounter.Interfaces;
using SnackCounter.Models.Entities;
using SnackCounter.Models.Enum;
using SnackCounter.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SnackCounter.Tests
{
    public class VolunteerReassignmentTests
    {
        [Fact]
        public async Task DeclineRefill_ShouldReassignToAnotherVolunteer()
        {
            var options = new DbContextOptionsBuilder<SnackCounterDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(w =>
                    w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using var context = new SnackCounterDbContext(options);

            var firstEmployee = new Employee
            {
                EmployeeId = "EMP001"
            };

            var secondEmployee = new Employee
            {
                EmployeeId = "EMP002"
            };

            context.Employees.AddRange(firstEmployee, secondEmployee);
            await context.SaveChangesAsync();

            var refill = new Refill
            {
                Office = OfficeType.Oslo,
                Status = RefillStatus.Assigned,
                AssignedEmployeeId = firstEmployee.Id
            };

            context.Refills.Add(refill);
            await context.SaveChangesAsync();

            var service = new RefillService(
                context,
                new FakeVolunteerService(secondEmployee),
                null!,
                new FakeChatService(),
                null!
            );

            await service.DeclineRefillAsync(refill.Id, firstEmployee.Id);

            var updatedRefill = await context.Refills.FindAsync(refill.Id);

            Assert.NotNull(updatedRefill);
            Assert.Equal(RefillStatus.Assigned, updatedRefill.Status);
            Assert.Equal(secondEmployee.Id, updatedRefill.AssignedEmployeeId);
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
