using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CognologyFlightBooking.Api.Entities
{
    public static class FlightInfoContextExtensions
    {
        public static void EnsureSeedDataForContext(this FlightInfoContext context)
        {
            if (context.Flights.Any())
            {
                return;
            }

            var fligts = new List<Flight>()
            {
                new Flight()
                {
                      FlightNumber = "F100",
                       PassengerCapacity = 500,
                        DeparterCity = "Melbourne",
                         ArrivalCity = "Sydney",
                          StartTime = new TimeSpan(13, 0, 0),
                           EndTime  = new TimeSpan(15, 0, 0)
                },
                new Flight()
                {
                      FlightNumber = "F200",
                       PassengerCapacity = 200,
                        DeparterCity = "Melbourne",
                         ArrivalCity = "Perth",
                          StartTime = new TimeSpan(20, 0, 0),
                           EndTime  = new TimeSpan(23, 0, 0)
                },
                 new Flight()
                {
                      FlightNumber = "F300",
                       PassengerCapacity = 200,
                        DeparterCity = "Melbourne",
                         ArrivalCity = "QueensLand",
                          StartTime = new TimeSpan(6, 0, 0),
                           EndTime  = new TimeSpan(12, 0, 0)
                }
            };

            context.Flights.AddRange(fligts);
            context.SaveChanges();
        }
    }
}
