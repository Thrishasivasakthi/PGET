using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        [Required]
        [ForeignKey("Booking")]
        public int BookingId { get; set; }

        public Booking? Booking { get; set; }

        [Required, StringLength(30)]
        public string PaymentMode { get; set; } // UPI, Card

        [Required, StringLength(30)]
        public string PaymentStatus { get; set; } // Success / Failed

        [Required]
        public DateTime PaymentDate { get; set; }
    }
}
