using Microsoft.EntityFrameworkCore;

namespace CognologyFlightBooking.Api.Entities
{
    public class FlightInfoContext : DbContext
    {
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<FlightBooking> FlightBookings { get; set; }

        public FlightInfoContext(DbContextOptions<FlightInfoContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
            Database.Migrate();
        }
    }
}
