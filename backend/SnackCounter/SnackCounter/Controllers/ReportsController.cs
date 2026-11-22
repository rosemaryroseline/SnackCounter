using Microsoft.AspNetCore.Mvc;
using SnackCounter.Interfaces;

namespace SnackCounter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController: ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        // GET: api/reports/monthly?year=2026&month=11
        [HttpGet("monthly")]
        public async Task<IActionResult> GetMonthlyReport(
            [FromQuery] int year,
            [FromQuery] int month)
        {
            if (year <= 0 || month < 1 || month > 12)
            {
                return BadRequest("Please provide a valid year and month.");
            }

            var report = await _reportService
                .GetMonthlyReportAsync(year, month);

            return Ok(report);
        }

    }
}
