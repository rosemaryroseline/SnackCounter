using SnackCounter.Models.Enum;

namespace SnackCounter.Models.Entities
{
    public class Reimbursement
    {
        public int Id { get; set; }

        public int PurchaseId { get; set; }

        public Purchase Purchase { get; set; } = null!;

        public string? HrClaimId { get; set; }

        public ReimbursementStatus Status { get; set; }
            = ReimbursementStatus.Pending;

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ApprovedAt { get; set; }

        public string? RejectionReason { get; set; }
    }
}
