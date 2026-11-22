namespace SnackCounter.Models.Entities
{
    public class Purchase
    {
        public int Id { get; set; }

        public int RefillId { get; set; }

        public Refill Refill { get; set; } = null!;

        public int EmployeeId { get; set; }

        public Employee Employee { get; set; } = null!;

        public decimal Amount { get; set; }

        public string Currency { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string? ReceiptPath { get; set; }

        public DateTime PurchasedAt { get; set; } = DateTime.UtcNow;

        public Reimbursement? Reimbursement { get; set; }
    }
}
