using DAL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusController : ControllerBase
    {
        private readonly IBusService _busService;

        public BusController(IBusService busService)
        {
            _busService = busService;
        }

        // api/bus/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBusById(int id)
        {
            var bus = await _busService.GetBusByIdAsync(id);
            if (bus == null)
                return NotFound(new { message = "Bus not found." });

            return Ok(bus);
        }

        // api/bus/route/5/date/2025-06-25
        [HttpGet("route/{routeId}/date/{date}")]
        public async Task<IActionResult> GetAvailableBuses(int routeId, DateTime date)
        {
            var buses = await _busService.GetAvailableBusesAsync(routeId, date);
            if (buses == null || !buses.Any())
                return NotFound(new { message = "No buses available." });

            return Ok(buses);
        }

        // api/bus/seats/3
        [HttpGet("seats/{busId}")]
        public async Task<IActionResult> GetAvailableSeats(int busId)
        {
            var seats = await _busService.GetAvailableSeatsAsync(busId);
            return Ok(seats);
        }

        // api/bus/add
        [HttpPost("add")]
        public async Task<IActionResult> AddBus([FromBody] Bus bus)
        {
            try
            {
                await _busService.AddBusAsync(bus);
                return Ok(new { message = "Bus added successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // api/bus/update
        [HttpPut("update")]
        public async Task<IActionResult> UpdateBus([FromBody] Bus bus)
        {
            try
            {
                await _busService.UpdateBusAsync(bus);
                return Ok(new { message = "Bus updated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
