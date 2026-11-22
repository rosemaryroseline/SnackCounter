using SnackCounter.Models.Entities;
using SnackCounter.Models.Enum;

namespace SnackCounter.Interfaces
{
    public interface IVolunteerService
    {
        Task<Employee?> SelectVolunteerAsync(OfficeType office);

    }
}
