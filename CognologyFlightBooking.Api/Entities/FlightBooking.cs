using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CognologyFlightBooking.Api.Entities
{
    public class FlightBooking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public string ArrivalCity { get; set; }
        [Required]
        public string DeparterCity { get; set; }
        [Required]
        public Int16 NumberOfPassengers { get; set; }
        [Required]
        [ForeignKey("FlightId")]
        public Flight Flight { get; set; }
        public int FlightId { get; set; }
        [Required]
        [ForeignKey("PassengerId")]
        public Passenger Passenger { get; set; }
        public int PassengerId { get; set; }
    }
}
