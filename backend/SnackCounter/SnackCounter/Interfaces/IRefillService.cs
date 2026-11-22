using SnackCounter.Models.DTOs;
using SnackCounter.Models.Entities;

namespace SnackCounter.Interfaces
{
    public interface IRefillService
    {
        Task<Refill> CreateRefillAsync(CreateRefillRequest request);

        Task<List<Refill>> GetRefillsAsync();

        Task<Refill> AcceptRefillAsync(int refillId, int employeeId);

        Task<Refill> DeclineRefillAsync(int refillId, int employeeId);

        Task<Refill> SubmitPurchaseAsync(
            int refillId,
            SubmitPurchaseRequest request);
    }
}
