using DAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Authorize(Roles = "User")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly ICancellationService _cancellationService;

        private readonly ILogger<BookingController> _logger;

        public BookingController(
            IBookingService bookingService,
            ICancellationService cancellationService,
            ILogger<BookingController> logger)
        {
            _bookingService = bookingService;
            _cancellationService = cancellationService;
            _logger = logger;
        }

        // api/booking/book
        // This endpoint books a ticket for a user on a specific bus
        [HttpPost("book")]
        public async Task<IActionResult> BookTicket([FromQuery] int userId, [FromQuery] int busId, [FromBody] List<string> seatNumbers)
        {
            _logger.LogInformation("User {UserId} is attempting to book {Count} seat(s) on Bus {BusId}.", userId, seatNumbers.Count, busId);
            try
            {
                var booking = await _bookingService.BookTicketAsync(userId, busId, seatNumbers);
                _logger.LogInformation("Booking successful. Booking ID: {BookingId}.", booking.Id);
                return Ok(booking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Booking failed for User {UserId} on Bus {BusId}.", userId, busId);
                return BadRequest(new { message = ex.Message });
            }
        }

        // api/booking/cancel/5?reason=changed mind
        // This endpoint cancels a booking by its ID and records the cancellation reason
        [HttpPut("cancel/{bookingId}")]
        public async Task<IActionResult> CancelBooking(int bookingId, [FromQuery] string reason)
        {
            _logger.LogInformation("Request received to cancel Booking ID: {BookingId} with reason: {Reason}.", bookingId, reason);

            try
            {
                var booking = await _bookingService.GetBookingByIdAsync(bookingId);
                if (booking == null)
                {
                    _logger.LogWarning("Cancellation failed: Booking ID {BookingId} not found.", bookingId);
                    return NotFound(new { message = "Booking not found" });
                }

                var success = await _bookingService.CancelBookingAsync(bookingId, reason);
                if (!success)
                {
                    _logger.LogWarning("Cancellation failed for Booking ID: {BookingId}.", bookingId);
                    return BadRequest(new { message = "Cancellation failed" });
                }

                // Record cancellation separately
                await _cancellationService.RecordCancellationAsync(bookingId, reason, booking.TotalAmount);
                _logger.LogInformation("Cancellation successful for Booking ID: {BookingId}.", bookingId);

                return Ok(new { message = "Booking cancelled successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while cancelling Booking ID: {BookingId}.", bookingId);
                return BadRequest(new { message = ex.Message });
            }
        }

        // api/booking/7
        // This endpoint retrieves booking details by booking ID
        [HttpGet("{bookingId}")]
        public async Task<IActionResult> GetBookingById(int bookingId)
        {
            _logger.LogInformation("Fetching Booking details for Booking ID: {BookingId}.", bookingId);
            var booking = await _bookingService.GetBookingByIdAsync(bookingId);
            if (booking == null)
            {
                _logger.LogWarning("Booking ID: {BookingId} not found.", bookingId);
                return NotFound(new { message = "Booking not found" });
            }

            _logger.LogInformation("Booking ID: {BookingId} retrieved successfully.", bookingId);
            return Ok(booking);
        }

        // api/booking/user/3
        // This endpoint retrieves all bookings made by a specific user
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetBookingsByUser(int userId)
        {
            _logger.LogInformation("Fetching all bookings for User ID: {UserId}.", userId);
            var bookings = await _bookingService.GetBookingsByUserAsync(userId);
            _logger.LogInformation("Found {Count} bookings for User ID: {UserId}.", bookings.Count(), userId);
            return Ok(bookings);
        }
    }
}
