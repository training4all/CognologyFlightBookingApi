using System.ComponentModel.DataAnnotations;

namespace CognologyFlightBooking.Api.Models
{
    public class FlightForMakingBookingDto
    {
        [Required]
        [MaxLength(10)]
        public string FlightNumber { get; set; }
    }
}
