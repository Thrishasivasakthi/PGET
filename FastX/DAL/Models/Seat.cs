using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Seat
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Bus")]
        public int BusId { get; set; }

        public Bus? Bus { get; set; }

        [Required, StringLength(10)]
        public string SeatNumber { get; set; }

        public bool IsBooked { get; set; }
    }
}
