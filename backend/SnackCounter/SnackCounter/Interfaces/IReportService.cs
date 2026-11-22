using SnackCounter.Models.DTOs;

namespace SnackCounter.Interfaces
{
    public interface IReportService
    {
        Task<List<MonthlyReportDto>> GetMonthlyReportAsync(
       int year,
       int month);
    }
}
