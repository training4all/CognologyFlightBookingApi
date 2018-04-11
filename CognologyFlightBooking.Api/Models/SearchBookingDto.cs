using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CognologyFlightBooking.Api.Models
{
    public class SearchBookingDto
    {
        public DateTime Date { get; set; }
        [Required]
        [MaxLength(50)]
        public string ArrivalCity { get; set; }
        [Required]
        [MaxLength(50)]
        public string DeparterCity { get; set; }
        [Required]
        [Range(1, 1000)]
        public Int16 NumberOfPassengers { get; set; }
        [Required]
        [MaxLength(50)]
        public string PassengerName { get; set; }
        public string FlightNumber { get; set; }
    }
}
