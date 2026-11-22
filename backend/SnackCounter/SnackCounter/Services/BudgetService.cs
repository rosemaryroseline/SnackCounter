using SnackCounter.Data;
using SnackCounter.Interfaces;
using SnackCounter.Models.Enum;
using Microsoft.EntityFrameworkCore;


namespace SnackCounter.Services
{
    public class BudgetService: IBudgetService
    {
        private readonly SnackCounterDbContext _context;

        public BudgetService(SnackCounterDbContext context)
        {
            _context = context;
        }

        public async Task<(decimal Budget, decimal Spent,
            decimal Remaining, string Currency)> GetBudgetAsync(
                OfficeType office)
        {
            var budget = await _context.OfficeBudgets
                .FirstOrDefaultAsync(b => b.Office == office);

            if (budget == null)
                throw new InvalidOperationException(
                    $"Budget is not configured for {office}.");

            var now = DateTime.UtcNow;

            var monthStart = new DateTime(
                now.Year, now.Month, 1);

            var nextMonthStart = monthStart.AddMonths(1);

            var spent = await _context.Purchases
                .Where(p =>
                    p.Refill.Office == office &&
                    p.PurchasedAt >= monthStart &&
                    p.PurchasedAt < nextMonthStart)
                .SumAsync(p => (decimal?)p.Amount) ?? 0m;

            var remaining = budget.MonthlyBudget - spent;

            return (
                budget.MonthlyBudget,
                spent,
                remaining,
                budget.Currency
            );
        }

        public async Task ValidatePurchaseAsync(
            OfficeType office,
            decimal amount)
        {
            if (amount <= 0)
            {
                throw new InvalidOperationException(
                    "Purchase amount must be greater than zero.");
            }

            var budgetInfo = await GetBudgetAsync(office);

            if (amount > budgetInfo.Remaining)
            {
                throw new InvalidOperationException(
                    $"Purchase exceeds the remaining budget. " +
                    $"Remaining budget: {budgetInfo.Remaining} " +
                    $"{budgetInfo.Currency}");
            }
        }
    }
}
