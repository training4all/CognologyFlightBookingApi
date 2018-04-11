using System;
using System.ComponentModel.DataAnnotations;

namespace CognologyFlightBooking.Api.Models
{
    public class FlightDto
    { 
        [Required]
        [MaxLength(10)]
        public string FlightNumber { get; set; }
        [Required]
        public TimeSpan StartTime { get; set; }
        [Required]
        public TimeSpan EndTime { get; set; }
        [Required]
        [Range(1, 1000)]
        public Int16 PassengerCapacity { get; set; }
        [Required]
        [MaxLength(50)]
        public string DeparterCity { get; set; }
        [Required]
        [MaxLength(50)]
        public string ArrivalCity { get; set; }
    }
}
