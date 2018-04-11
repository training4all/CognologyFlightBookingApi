using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CognologyFlightBooking.Api.Data.Entities
{
    public class FlightBooking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
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
        [ForeignKey("FlightId")]
        public Flight Flight { get; set; }
       
        [Required]
        [ForeignKey("PassengerId")]
        public Passenger Passenger { get; set; }
       
    }
}
