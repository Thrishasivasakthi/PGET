using DAL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IRouteService _routeService;

        private readonly ILogger<AdminController> _logger;

        public AdminController(
            IAdminService adminService,
            IRouteService routeService,
            ILogger<AdminController> logger)
        {
            _adminService = adminService;
            _routeService = routeService;
            _logger = logger;
        }

        // api/admin/users
        // This endpoint retrieves all users in the system.
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            _logger.LogInformation("Fetching all users...");

            var users = await _adminService.GetAllUsersAsync();
            _logger.LogInformation("Fetched {Count} users.", users.Count());

            return Ok(users);
        }

        // api/admin/operators
        // This endpoint retrieves all operators in the system.
        [HttpGet("operators")]
        public async Task<IActionResult> GetAllOperators()
        {
            _logger.LogInformation("Admin requested all operators.");
            var operators = await _adminService.GetAllOperatorsAsync();
            _logger.LogInformation("Retrieved {Count} operators.", operators.Count());
            return Ok(operators);
        }

        // api/admin/user/4
        // This endpoint retrieves a specific user by ID.
        [HttpDelete("user/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            _logger.LogWarning("Admin attempting to delete user with ID {UserId}.", userId);
            var success = await _adminService.DeleteUserAsync(userId);
            if (!success)
            {
                _logger.LogError("Failed to delete user with ID {UserId}.", userId);
                return NotFound(new { message = "User not found or could not be deleted." });
            }

            _logger.LogInformation("User with ID {UserId} deleted successfully.", userId);
            return Ok(new { message = "User deleted successfully." });
        }

        // api/admin/operator/3
        // This endpoint retrieves a specific operator by ID.
        [HttpDelete("operator/{operatorId}")]
        public async Task<IActionResult> DeleteOperator(int operatorId)
        {
            _logger.LogWarning("Admin attempting to delete operator with ID {OperatorId}.", operatorId);
            var success = await _adminService.DeleteOperatorAsync(operatorId);
            if (!success)
            {
                _logger.LogError("Failed to delete operator with ID {OperatorId}.", operatorId);
                return NotFound(new { message = "Operator not found or could not be deleted." });
            }

            _logger.LogInformation("Operator with ID {OperatorId} deleted successfully.", operatorId);
            return Ok(new { message = "Operator deleted successfully." });
        }

        // api/admin/bookings
        // This endpoint retrieves all bookings in the system.
        [HttpGet("bookings")]
        public async Task<IActionResult> GetAllBookings()
        {
            _logger.LogInformation("Admin requested all bookings.");
            var bookings = await _adminService.GetAllBookingsAsync();
            _logger.LogInformation("Retrieved {Count} bookings.", bookings.Count());
            return Ok(bookings);
        }

        // api/admin/add-route
        // This endpoint allows the admin to add a new route.
        [HttpPost("add-route")]
        public async Task<IActionResult> AddRoute([FromBody] DAL.Models.Route route) 
        {
            try
            {
                _logger.LogInformation("Admin attempting to add a new route from {Origin} to {Destination}.",
                    route.Origin, route.Destination);
                var result = await _routeService.AddRouteAsync(route);

                _logger.LogInformation("Route added successfully with ID {RouteId}.", result.Id);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a new route.");
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
