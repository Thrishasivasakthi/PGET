using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }

        [Required]
        [ForeignKey("Bus")]
        public int BusId { get; set; }

        public Bus Bus { get; set; }

        [Required]
        public string SeatNumbers { get; set; } // e.g., "A1,A2"

        [Required]
        public DateTime BookingDate { get; set; }

        [Required, Range(0, 50000)]
        public decimal TotalAmount { get; set; }

        [Required]
        public string Status { get; set; } // Booked / Cancelled

        public Payment? Payment { get; set; }
        public Cancellation? Cancellation { get; set; }
    }
}
