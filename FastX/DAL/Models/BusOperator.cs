using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class BusOperator
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public User? User { get; set; }

        [Required, StringLength(100)]
        public string CompanyName { get; set; }

        [Required, EmailAddress]
        public string SupportEmail { get; set; }

        [Required, Phone]
        public string ContactNumber { get; set; }

        public ICollection<Bus>? Buses { get; set; }
    }
}
