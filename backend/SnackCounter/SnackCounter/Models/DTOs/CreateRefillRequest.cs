using SnackCounter.Models.Enum;

namespace SnackCounter.Models.DTOs
{
    public class CreateRefillRequest
    {
        public OfficeType Office { get; set; }

        public string CreatedByEmployeeId { get; set; } = string.Empty;
    }
}
