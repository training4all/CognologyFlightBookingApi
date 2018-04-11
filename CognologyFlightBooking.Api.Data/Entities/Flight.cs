using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace CognologyFlightBooking.Api.Data.Entities
{
    public class Flight
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string FlightNumber { get; set; }
        [Required]
        public TimeSpan StartTime { get; set; }
        [Required]
        public TimeSpan EndTime { get; set; }
        [Required]
        [Range(1, 1000)]
        public Int16  PassengerCapacity { get; set; }
        [Required]
        [MaxLength(50)]
        public string DeparterCity { get; set; }
        [Required]
        [MaxLength(50)]
        public string ArrivalCity { get; set; }

        public ICollection<FlightBooking> FlightBookings { get; set; }
    }
}
