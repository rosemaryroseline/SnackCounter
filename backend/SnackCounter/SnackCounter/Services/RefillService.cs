using SnackCounter.Data;
using SnackCounter.Interfaces;
using SnackCounter.Models.DTOs;
using SnackCounter.Models.Entities;
using SnackCounter.Models.Enum;
using Microsoft.EntityFrameworkCore;


namespace SnackCounter.Services
{
    public class RefillService: IRefillService
    {
        private readonly SnackCounterDbContext _context;
        private readonly IVolunteerService _volunteerService;
        private readonly IBudgetService _budgetService;
        private readonly IChatService _chatService;
        private readonly IReimbursementService _reimbursementService;

        public RefillService(
            SnackCounterDbContext context,
            IVolunteerService volunteerService,
            IBudgetService budgetService,
            IChatService chatService,
            IReimbursementService reimbursementService)
        {
            _context = context;
            _volunteerService = volunteerService;
            _budgetService = budgetService;
            _chatService = chatService;
            _reimbursementService = reimbursementService;
        }

        // 1. CREATE REFILL
        public async Task<Refill> CreateRefillAsync(
            CreateRefillRequest request)
        {
            // Transaction helps protect the creation operation.
            await using var transaction =
                await _context.Database.BeginTransactionAsync(
                    System.Data.IsolationLevel.Serializable);

            var activeRefillExists = await _context.Refills
                .AnyAsync(r =>
                    r.Office == request.Office &&
                    (r.Status == RefillStatus.Open ||
                     r.Status == RefillStatus.Assigned ||
                     r.Status == RefillStatus.Accepted));

            if (activeRefillExists)
            {
                throw new InvalidOperationException(
                    $"An active refill already exists for {request.Office}.");
            }

            var refill = new Refill
            {
                Office = request.Office,
                CreatedByEmployeeId = request.CreatedByEmployeeId,
                Status = RefillStatus.Open,
                CreatedAt = DateTime.UtcNow
            };

            _context.Refills.Add(refill);

            await _context.SaveChangesAsync();

            await AssignVolunteerAsync(refill);

            await transaction.CommitAsync();

            return refill;
        }

        // 2. GET ALL REFILLS
        public async Task<List<Refill>> GetRefillsAsync()
        {
            return await _context.Refills
                .Include(r => r.AssignedEmployee)
                .Include(r => r.Purchase)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        // 3. ACCEPT REFILL
        public async Task<Refill> AcceptRefillAsync(
            int refillId,
            int employeeId)
        {
            var refill = await _context.Refills
                .Include(r => r.AssignedEmployee)
                .FirstOrDefaultAsync(r => r.Id == refillId);

            if (refill == null)
                throw new KeyNotFoundException("Refill not found.");

            if (refill.AssignedEmployeeId != employeeId)
                throw new InvalidOperationException(
                    "This refill is not assigned to you.");

            if (refill.Status != RefillStatus.Assigned)
                throw new InvalidOperationException(
                    "This refill cannot be accepted.");

            refill.Status = RefillStatus.Accepted;
            refill.AcceptedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return refill;
        }

        // 4. DECLINE REFILL
        public async Task<Refill> DeclineRefillAsync(
            int refillId,
            int employeeId)
        {
            var refill = await _context.Refills
                .FirstOrDefaultAsync(r => r.Id == refillId);

            if (refill == null)
                throw new KeyNotFoundException("Refill not found.");

            if (refill.AssignedEmployeeId != employeeId)
                throw new InvalidOperationException(
                    "This refill is not assigned to you.");

            if (refill.Status != RefillStatus.Assigned)
                throw new InvalidOperationException(
                    "Only assigned refills can be declined.");

            // Remove the current volunteer assignment.
            refill.AssignedEmployeeId = null;
            refill.Status = RefillStatus.Open;

            await _context.SaveChangesAsync();

            // Find another volunteer.
            await AssignVolunteerAsync(refill);

            return refill;
        }

        // 5. SUBMIT PURCHASE
        public async Task<Refill> SubmitPurchaseAsync(
            int refillId,
            SubmitPurchaseRequest request)
        {
            var refill = await _context.Refills
                .Include(r => r.Purchase)
                .FirstOrDefaultAsync(r => r.Id == refillId);

            if (refill == null)
                throw new KeyNotFoundException("Refill not found.");

            if (refill.Status != RefillStatus.Accepted)
                throw new InvalidOperationException(
                    "Refill must be accepted before purchase submission.");

            if (refill.AssignedEmployeeId != request.EmployeeId)
                throw new InvalidOperationException(
                    "Only the assigned volunteer can submit the purchase.");

            if (refill.Purchase != null)
                throw new InvalidOperationException(
                    "Purchase has already been submitted.");

            // Validate the office budget.
            await _budgetService.ValidatePurchaseAsync(
                refill.Office, request.Amount);

            // Currency is determined by office.
            var currency = refill.Office == OfficeType.Oslo
                ? "NOK"
                : "INR";

            var purchase = new Purchase
            {
                RefillId = refill.Id,
                EmployeeId = request.EmployeeId,
                Amount = request.Amount,
                Currency = currency,
                Description = request.Description,
                ReceiptPath = request.ReceiptPath,
                PurchasedAt = DateTime.UtcNow
            };

            _context.Purchases.Add(purchase);

            refill.Status = RefillStatus.ReimbursementPending;

            await _context.SaveChangesAsync();

            // Submit the claim to the mock HR service.
            await _reimbursementService.SubmitAsync(purchase);

            // Notify volunteer.
            await _chatService.SendMessageAsync(
                request.EmployeeId.ToString(),
                "Your purchase was recorded and reimbursement " +
                "was submitted to HR.");

            return refill;
        }

        // PRIVATE METHOD: ASSIGN VOLUNTEER
        private async Task AssignVolunteerAsync(Refill refill)
        {
            var volunteer = await _volunteerService
                .SelectVolunteerAsync(refill.Office);

            if (volunteer == null)
            {
                refill.Status = RefillStatus.Open;
                refill.AssignedEmployeeId = null;

                await _context.SaveChangesAsync();

                return;
            }

            refill.AssignedEmployeeId = volunteer.Id;
            refill.Status = RefillStatus.Assigned;
            refill.AssignmentExpiresAt = DateTime.UtcNow.AddHours(24);

            await _context.SaveChangesAsync();

            // Notify selected volunteer.
            await _chatService.SendMessageAsync(
                volunteer.EmployeeId,
                $"Snack refill requested for {refill.Office}. " +
                "Please accept or decline within 24 hours.");
        }
    }
}
