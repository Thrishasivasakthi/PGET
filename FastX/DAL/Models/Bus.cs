using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL.Models
{
    public class Bus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        [Required, StringLength(100)]
        public string BusName { get; set; }

        [Required, StringLength(20)]
        public string BusNumber { get; set; }

        [Required, StringLength(50)]
        public string BusType { get; set; } // "Sleeper AC", etc.

        [Required, Range(10, 100)]
        public int TotalSeats { get; set; }

        [Required]
        [ForeignKey("Route")]
        public int RouteId { get; set; }

        public Route? Route { get; set; }

        [Required]
        [ForeignKey("BusOperator")]
        public int BusOperatorId { get; set; }

        public BusOperator? BusOperator { get; set; }

        public ICollection<Seat>? Seats { get; set; }
        public ICollection<BusAmenity>? BusAmenities { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
    }
}
