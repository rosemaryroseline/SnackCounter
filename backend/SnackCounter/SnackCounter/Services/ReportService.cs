using SnackCounter.Data;
using SnackCounter.Interfaces;
using SnackCounter.Models.DTOs;
using SnackCounter.Models.Enum;
using Microsoft.EntityFrameworkCore;


namespace SnackCounter.Services
{
    public class ReportService: IReportService
    {
        private readonly SnackCounterDbContext _context;

        public ReportService(SnackCounterDbContext context)
        {
            _context = context;
        }

        public async Task<List<MonthlyReportDto>> GetMonthlyReportAsync(
            int year,
            int month)
        {
            if (month < 1 || month > 12)
                throw new ArgumentException("Invalid month.");

            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1);

            var budgets = await _context.OfficeBudgets
                .ToListAsync();

            var reports = new List<MonthlyReportDto>();

            foreach (var office in Enum.GetValues<OfficeType>())
            {
                var budget = budgets.FirstOrDefault(
                    b => b.Office == office);

                if (budget == null)
                    continue;

                var purchases = await _context.Purchases
                    .Where(p =>
                        p.Refill.Office == office &&
                        p.PurchasedAt >= startDate &&
                        p.PurchasedAt < endDate)
                    .ToListAsync();

                var totalSpent = purchases.Sum(p => p.Amount);

                var pendingReimbursements = await _context.Reimbursements
                    .CountAsync(r =>
                        r.Purchase.Refill.Office == office &&
                        r.Status == ReimbursementStatus.Pending &&
                        r.Purchase.PurchasedAt >= startDate &&
                        r.Purchase.PurchasedAt < endDate);

                reports.Add(new MonthlyReportDto
                {
                    Office = office.ToString(),
                    Currency = budget.Currency,
                    MonthlyBudget = budget.MonthlyBudget,
                    TotalSpent = totalSpent,
                    RemainingBudget = budget.MonthlyBudget - totalSpent,
                    TotalRefills = purchases.Count,
                    PendingReimbursements = pendingReimbursements
                });
            }

            return reports;
        }
    }
}
