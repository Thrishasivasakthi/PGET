using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
     public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Role { get; set; } // "User", "Admin", "Operator"

        public ICollection<Booking>? Bookings { get; set; }
        public BusOperator? BusOperatorProfile { get; set; }
    }
}
