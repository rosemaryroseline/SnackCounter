using SnackCounter.Data;
using SnackCounter.Interfaces;
using SnackCounter.Models.Entities;
using Microsoft.EntityFrameworkCore;


namespace SnackCounter.Services
{
    public class ReimbursementService: IReimbursementService
    {
        private readonly SnackCounterDbContext _context;

        public ReimbursementService(SnackCounterDbContext context)
        {
            _context = context;
        }

        public async Task<Reimbursement> SubmitAsync(Purchase purchase)
        {
            // Simulate sending a claim to an external HR system.
            var mockHrClaimId = $"HR-{Guid.NewGuid():N}"[..15];

            var reimbursement = new Reimbursement
            {
                PurchaseId = purchase.Id,
                HrClaimId = mockHrClaimId,
                Status = Models.Enum.ReimbursementStatus.Pending,
                SubmittedAt = DateTime.UtcNow
            };

            _context.Reimbursements.Add(reimbursement);

            await _context.SaveChangesAsync();

            Console.WriteLine(
                $"Mock HR claim submitted: {mockHrClaimId}");

            return reimbursement;
        }

        public async Task<List<Reimbursement>> GetAllAsync()
        {
            return await _context.Reimbursements
                .Include(r => r.Purchase)
                .OrderByDescending(r => r.SubmittedAt)
                .ToListAsync();
        }
    }
}
