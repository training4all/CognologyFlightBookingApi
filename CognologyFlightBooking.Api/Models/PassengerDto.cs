using System.ComponentModel.DataAnnotations;

namespace CognologyFlightBooking.Api.Models
{
    public class PassengerDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(20)]
        public string Mobile { get; set; }
    }
}
