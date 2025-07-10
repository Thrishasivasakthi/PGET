using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Cancellation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Booking")]
        public int BookingId { get; set; }

        public Booking? Booking { get; set; }

        [Required, StringLength(200)]
        public string Reason { get; set; }

        [Required]
        public DateTime CancelledOn { get; set; }

        [Required, Range(0, 50000)]
        public decimal RefundAmount { get; set; }
    }
}
