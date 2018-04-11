using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CognologyFlightBooking.Api.Entities
{
    public class Flight
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string FlightNumber { get; set; }
        [Required]
        public TimeSpan StartTime { get; set; }
        [Required]
        public TimeSpan EndTime { get; set; }
        [Required]
        public Int16  PassengerCapacity { get; set; }
        [Required]
        public string DeparterCity { get; set; }
        [Required]
        public string ArrivalCity { get; set; }

    }
}
