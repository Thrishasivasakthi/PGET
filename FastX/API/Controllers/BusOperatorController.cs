using DAL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class BusOperatorController : ControllerBase
    {
        private readonly IBusOperatorService _busOperatorService;

        public BusOperatorController(IBusOperatorService busOperatorService)
        {
            _busOperatorService = busOperatorService;
        }

        // api/busoperator/profile/2
        [HttpGet("profile/{userId}")]
        public async Task<IActionResult> GetProfile(int userId)
        {
            var profile = await _busOperatorService.GetProfileAsync(userId);
            if (profile == null)
                return NotFound(new { message = "Operator profile not found." });

            return Ok(profile);
        }

        // api/busoperator/buses/3
        [HttpGet("buses/{operatorId}")]
        public async Task<IActionResult> GetOperatorBuses(int operatorId)
        {
            var buses = await _busOperatorService.GetOperatorBusesAsync(operatorId);
            return Ok(buses);
        }

        // api/busoperator/bookings/3
        [HttpGet("bookings/{operatorId}")]
        public async Task<IActionResult> GetOperatorBookings(int operatorId)
        {
            var bookings = await _busOperatorService.GetOperatorBookingsAsync(operatorId);
            return Ok(bookings);
        }

        // api/busoperator/add-bus
        [HttpPost("add-bus")]
        public async Task<IActionResult> AddBus([FromBody] Bus bus)
        {
            try
            {
                await _busOperatorService.AddBusAsync(bus);
                return Ok(new { message = "Bus added successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // api/busoperator/edit-bus
        [HttpPut("edit-bus")]
        public async Task<IActionResult> EditBus([FromBody] Bus bus)
        {
            try
            {
                await _busOperatorService.EditBusAsync(bus);
                return Ok(new { message = "Bus updated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // api/busoperator/refund/10
        [HttpPut("refund/{bookingId}")]
        public async Task<IActionResult> RefundBooking(int bookingId)
        {
            var result = await _busOperatorService.RefundBookingAsync(bookingId);
            if (!result)
                return BadRequest(new { message = "Refund failed or already processed." });

            return Ok(new { message = "Refund issued successfully." });
        }
    }
}
