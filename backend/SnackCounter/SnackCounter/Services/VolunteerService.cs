using SnackCounter.Data;
using SnackCounter.Interfaces;
using SnackCounter.Models.Entities;
using SnackCounter.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace SnackCounter.Services
{
    public class VolunteerService: IVolunteerService
    {
        private readonly SnackCounterDbContext _context;

        public VolunteerService(SnackCounterDbContext context)
        {
            _context = context;
        }

        public async Task<Employee?> SelectVolunteerAsync(OfficeType office)
        {
            var employees = await _context.Employees
                .Where(e => e.Office == office
                         && e.IsActive
                         && e.IsAvailable)
                .ToListAsync();

            var availableEmployees = new List<Employee>();

            foreach (var employee in employees)
            {
                var hasActiveAssignment = await _context.Refills
                    .AnyAsync(r =>
                        r.AssignedEmployeeId == employee.Id &&
                        (r.Status == RefillStatus.Assigned ||
                         r.Status == RefillStatus.Accepted));

                if (!hasActiveAssignment)
                {
                    availableEmployees.Add(employee);
                }
            }

            if (!availableEmployees.Any())
                return null;

            var employeeRefillCounts = new Dictionary<int, int>();

            foreach (var employee in availableEmployees)
            {
                var completedCount = await _context.Refills
                    .CountAsync(r =>
                        r.AssignedEmployeeId == employee.Id &&
                        r.Status == RefillStatus.Completed);

                employeeRefillCounts[employee.Id] = completedCount;
            }

            return availableEmployees
                .OrderBy(e => employeeRefillCounts[e.Id])
                .ThenBy(e => e.Id)
                .FirstOrDefault();
        }
    }
}
