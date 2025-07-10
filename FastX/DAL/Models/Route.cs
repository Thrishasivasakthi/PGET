using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Route
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Origin { get; set; }

        [Required, StringLength(100)]
        public string Destination { get; set; }

        [Required]
        public DateTime DepartureTime { get; set; }

        [Required]
        public DateTime ArrivalTime { get; set; }

        [Required, Range(0, 10000)]
        public decimal Fare { get; set; }

        public ICollection<Bus> Buses { get; set; }
    }
}
