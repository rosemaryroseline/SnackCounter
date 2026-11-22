namespace SnackCounter.Models.DTOs
{
    public class SubmitPurchaseRequest
    {
        public int EmployeeId { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; } = string.Empty;

        public string? ReceiptPath { get; set; }
    }
}
