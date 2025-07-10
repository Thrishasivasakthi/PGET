using DAL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Roles = "Admin,BusOperator")]
    [ApiController]
    [Route("api/[controller]")]
    public class AmenityController : ControllerBase
    {
        private readonly IAmenityService _amenityService;

        public AmenityController(IAmenityService amenityService)
        {
            _amenityService = amenityService;
        }

        // api/amenity/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAllAmenities()
        {
            var amenities = await _amenityService.GetAllAmenitiesAsync();
            return Ok(amenities);
        }

        // api/amenity/bus/5
        [HttpGet("bus/{busId}")]
        public async Task<IActionResult> GetAmenitiesByBus(int busId)
        {
            var busAmenities = await _amenityService.GetAmenitiesByBusIdAsync(busId);
            return Ok(busAmenities);
        }

        // api/amenity/assign
        [HttpPost("assign")]
        public async Task<IActionResult> AssignAmenitiesToBus([FromQuery] int busId, [FromBody] List<int> amenityIds)
        {
            try
            {
                await _amenityService.AssignAmenitiesToBusAsync(busId, amenityIds);
                return Ok(new { message = "Amenities assigned successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
