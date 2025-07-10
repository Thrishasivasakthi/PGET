using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class BusAmenity
    {
        [ForeignKey("Bus")]
        public int BusId { get; set; }

        public Bus Bus { get; set; }

        [ForeignKey("Amenity")]
        public int AmenityId { get; set; }

        public Amenity Amenity { get; set; }
    }
}
