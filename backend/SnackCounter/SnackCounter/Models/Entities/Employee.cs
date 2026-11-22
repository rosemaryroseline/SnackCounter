using SnackCounter.Models.Enum;

namespace SnackCounter.Models.Entities
{
    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string EmployeeId { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public OfficeType Office { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsAvailable { get; set; } = true;

        public ICollection<Refill> AssignedRefills { get; set; }
            = new List<Refill>();
    }
}
