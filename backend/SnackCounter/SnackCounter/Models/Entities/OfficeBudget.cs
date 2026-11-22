using SnackCounter.Models.Enum;

namespace SnackCounter.Models.Entities
{
    public class OfficeBudget
    {
        public int Id { get; set; }

        public OfficeType Office { get; set; }

        public decimal MonthlyBudget { get; set; }

        public string Currency { get; set; } = string.Empty;
    }
}
