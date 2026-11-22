using SnackCounter.Models.Entities;
using SnackCounter.Models.Enum;

namespace SnackCounter.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(
       SnackCounterDbContext context)
        {
            if (!context.OfficeBudgets.Any())
            {
                context.OfficeBudgets.AddRange(
                    new OfficeBudget
                    {
                        Office = OfficeType.Oslo,
                        MonthlyBudget = 6000,
                        Currency = "NOK"
                    },
                    new OfficeBudget
                    {
                        Office = OfficeType.Bengaluru,
                        MonthlyBudget = 15000,
                        Currency = "INR"
                    });
            }

            if (!context.Employees.Any())
            {
                context.Employees.AddRange(
                    new Employee
                    {
                        Name = "Oslo Employee 1",
                        EmployeeId = "OSL001",
                        Email = "oslo1@company.com",
                        Office = OfficeType.Oslo
                    },
                    new Employee
                    {
                        Name = "Oslo Employee 2",
                        EmployeeId = "OSL002",
                        Email = "oslo2@company.com",
                        Office = OfficeType.Oslo
                    },
                    new Employee
                    {
                        Name = "Bengaluru Employee 1",
                        EmployeeId = "BLR001",
                        Email = "blr1@company.com",
                        Office = OfficeType.Bengaluru
                    },
                    new Employee
                    {
                        Name = "Bengaluru Employee 2",
                        EmployeeId = "BLR002",
                        Email = "blr2@company.com",
                        Office = OfficeType.Bengaluru
                    },
                    new Employee
                    {
                        Name = "Bengaluru Employee 3",
                        EmployeeId = "BLR003",
                        Email = "blr3@company.com",
                        Office = OfficeType.Bengaluru
                    });
            }

            await context.SaveChangesAsync();
        }
    }
}
