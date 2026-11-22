using SnackCounter.Models.Entities;

namespace SnackCounter.Interfaces
{
    public interface IReimbursementService
    {
        Task<Reimbursement> SubmitAsync(Purchase purchase);

        Task<List<Reimbursement>> GetAllAsync();
    }
}
