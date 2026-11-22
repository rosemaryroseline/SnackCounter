using SnackCounter.Models.Enum;

namespace SnackCounter.Interfaces
{
    public interface IBudgetService
    {
        Task<(decimal Budget, decimal Spent, decimal Remaining, string Currency)>
      GetBudgetAsync(OfficeType office);

        Task ValidatePurchaseAsync(OfficeType office, decimal amount);
    }
}
