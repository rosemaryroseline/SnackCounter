namespace SnackCounter.Models.DTOs
{
    public class MonthlyReportDto
    {
        public string Office { get; set; } = string.Empty;

        public string Currency { get; set; } = string.Empty;

        public decimal MonthlyBudget { get; set; }

        public decimal TotalSpent { get; set; }

        public decimal RemainingBudget { get; set; }

        public int TotalRefills { get; set; }

        public int PendingReimbursements { get; set; }
    }
}
