using DAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Roles = "User")]
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly ICancellationService _cancellationService;

        public BookingController(IBookingService bookingService, ICancellationService cancellationService)
        {
            _bookingService = bookingService;
            _cancellationService = cancellationService;
        }

        // api/booking/book
        [HttpPost("book")]
        public async Task<IActionResult> BookTicket([FromQuery] int userId, [FromQuery] int busId, [FromBody] List<string> seatNumbers)
        {
            try
            {
                var booking = await _bookingService.BookTicketAsync(userId, busId, seatNumbers);
                return Ok(booking);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // api/booking/cancel/5?reason=changed mind
        [HttpPut("cancel/{bookingId}")]
        public async Task<IActionResult> CancelBooking(int bookingId, [FromQuery] string reason)
        {
            try
            {
                var booking = await _bookingService.GetBookingByIdAsync(bookingId);
                if (booking == null)
                    return NotFound(new { message = "Booking not found" });

                var success = await _bookingService.CancelBookingAsync(bookingId, reason);
                if (!success)
                    return BadRequest(new { message = "Cancellation failed" });

                // Record cancellation separately
                await _cancellationService.RecordCancellationAsync(bookingId, reason, booking.TotalAmount);

                return Ok(new { message = "Booking cancelled successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // api/booking/7
        [HttpGet("{bookingId}")]
        public async Task<IActionResult> GetBookingById(int bookingId)
        {
            var booking = await _bookingService.GetBookingByIdAsync(bookingId);
            if (booking == null)
                return NotFound(new { message = "Booking not found" });

            return Ok(booking);
        }

        // api/booking/user/3
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetBookingsByUser(int userId)
        {
            var bookings = await _bookingService.GetBookingsByUserAsync(userId);
            return Ok(bookings);
        }
    }
}
