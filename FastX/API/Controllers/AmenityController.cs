using DAL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Authorize(Roles = "Admin,Operator")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class AmenityController : ControllerBase
    {
        private readonly IAmenityService _amenityService;

        private readonly ILogger<AmenityController> _logger;

        public AmenityController(IAmenityService amenityService, ILogger<AmenityController> logger)
        {
            _amenityService = amenityService;
            _logger = logger;
        }

        // api/amenity/all
        // this endpoint retrieves all amenities
        [HttpGet("all")]
        public async Task<IActionResult> GetAllAmenities()
        {
            _logger.LogInformation("Fetching all amenities...");
            var amenities = await _amenityService.GetAllAmenitiesAsync();
            _logger.LogInformation("Retrieved {Count} amenities.", amenities.Count());
            return Ok(amenities);
        }

        // api/amenity/bus/5
        // this endpoint retrieves amenities for a specific bus
        [HttpGet("bus/{busId}")]
        public async Task<IActionResult> GetAmenitiesByBus(int busId)
        {
            _logger.LogInformation("Fetching amenities for Bus ID: {BusId}", busId);
            var busAmenities = await _amenityService.GetAmenitiesByBusIdAsync(busId);
            _logger.LogInformation("Retrieved {Count} amenities for bus ID {BusId}.", busAmenities.Count(), busId);
            return Ok(busAmenities);
        }

        // api/amenity/assign
        // this endpoint assigns amenities to a bus
        [HttpPost("assign")]
        public async Task<IActionResult> AssignAmenitiesToBus([FromQuery] int busId, [FromBody] List<int> amenityIds)
        {
            _logger.LogInformation("Attempting to assign {Count} amenities to Bus ID: {BusId}.", amenityIds.Count, busId);
            try
            {
                await _amenityService.AssignAmenitiesToBusAsync(busId, amenityIds);
                _logger.LogInformation("Successfully assigned amenities to Bus ID: {BusId}.", busId);
                return Ok(new { message = "Amenities assigned successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to assign amenities to Bus ID: {BusId}", busId);
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
