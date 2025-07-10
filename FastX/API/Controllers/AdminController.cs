using DAL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IRouteService _routeService;

        public AdminController(IAdminService adminService, IRouteService routeService)
        {
            _adminService = adminService;
            _routeService = routeService;
        }

        // api/admin/users
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _adminService.GetAllUsersAsync();
            return Ok(users);
        }

        // api/admin/operators
        [HttpGet("operators")]
        public async Task<IActionResult> GetAllOperators()
        {
            var operators = await _adminService.GetAllOperatorsAsync();
            return Ok(operators);
        }

        // api/admin/user/4
        [HttpDelete("user/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var success = await _adminService.DeleteUserAsync(userId);
            if (!success)
                return NotFound(new { message = "User not found or could not be deleted." });

            return Ok(new { message = "User deleted successfully." });
        }

        // api/admin/operator/3
        [HttpDelete("operator/{operatorId}")]
        public async Task<IActionResult> DeleteOperator(int operatorId)
        {
            var success = await _adminService.DeleteOperatorAsync(operatorId);
            if (!success)
                return NotFound(new { message = "Operator not found or could not be deleted." });

            return Ok(new { message = "Operator deleted successfully." });
        }

        // api/admin/bookings
        [HttpGet("bookings")]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _adminService.GetAllBookingsAsync();
            return Ok(bookings);
        }

        
        [HttpPost("add-route")]
        public async Task<IActionResult> AddRoute([FromBody] DAL.Models.Route route) 
        {
            try
            {
                var result = await _routeService.AddRouteAsync(route); 
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
