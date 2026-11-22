using Microsoft.AspNetCore.Mvc;
using SnackCounter.Interfaces;
using SnackCounter.Models.DTOs;

namespace SnackCounter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RefillsController: ControllerBase
    {
        private readonly IRefillService _refillService;

        public RefillsController(IRefillService refillService)
        {
            _refillService = refillService;
        }

        // GET: api/refills
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var refills = await _refillService.GetRefillsAsync();

            return Ok(refills);
        }

        // POST: api/refills
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateRefillRequest request)
        {
            try
            {
                var refill = await _refillService.CreateRefillAsync(request);

                return CreatedAtAction(
                    nameof(GetAll),
                    new { id = refill.Id },
                    refill);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // POST: api/refills/{id}/accept
        [HttpPost("{id:int}/accept")]
        public async Task<IActionResult> Accept(
            int id,
            [FromQuery] int employeeId)
        {
            try
            {
                var refill = await _refillService
                    .AcceptRefillAsync(id, employeeId);

                return Ok(refill);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/refills/{id}/decline
        [HttpPost("{id:int}/decline")]
        public async Task<IActionResult> Decline(
            int id,
            [FromQuery] int employeeId)
        {
            try
            {
                var refill = await _refillService
                    .DeclineRefillAsync(id, employeeId);

                return Ok(refill);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/refills/{id}/purchase
        [HttpPost("{id:int}/purchase")]
        public async Task<IActionResult> SubmitPurchase(
            int id,
            [FromBody] SubmitPurchaseRequest request)
        {
            try
            {
                var refill = await _refillService
                    .SubmitPurchaseAsync(id, request);

                return Ok(refill);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
