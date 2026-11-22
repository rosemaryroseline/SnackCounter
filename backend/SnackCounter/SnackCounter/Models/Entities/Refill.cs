using SnackCounter.Models.Enum;

namespace SnackCounter.Models.Entities
{
    public class Refill
    {
        public int Id { get; set; }

        public OfficeType Office { get; set; }

        public RefillStatus Status { get; set; } = RefillStatus.Open;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? AcceptedAt { get; set; }

        public DateTime? AssignmentExpiresAt { get; set; }

        public DateTime? CompletedAt { get; set; }

        public int? AssignedEmployeeId { get; set; }

        public Employee? AssignedEmployee { get; set; }

        public string CreatedByEmployeeId { get; set; } = string.Empty;

        public Purchase? Purchase { get; set; }
    }
}
