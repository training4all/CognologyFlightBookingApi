using System;
using System.Collections.Generic;
using System.Linq;
using CognologyFlightBooking.Api.Data.Entities;

namespace CognologyFlightBooking.Api.Data
{
    public static class FlightInfoContextExtensions
    {
        private static void SeedFlightData(this FlightInfoContext context)
        {
            var fligts = new List<Flight>()
            {
                new Flight()
                {                    
                      FlightNumber = "F100",
                       PassengerCapacity = 23,
                        DeparterCity = "MEL",
                         ArrivalCity = "SYD",
                          StartTime = new TimeSpan(13, 0, 0),
                           EndTime  = new TimeSpan(15, 0, 0)
                },
                new Flight()
                {                 
                      FlightNumber = "F200",
                       PassengerCapacity = 20,
                        DeparterCity = "MEL",
                         ArrivalCity = "PER",
                          StartTime = new TimeSpan(20, 0, 0),
                           EndTime  = new TimeSpan(23, 0, 0)
                },
                 new Flight()
                {                 
                      FlightNumber = "F300",
                       PassengerCapacity = 20,
                        DeparterCity = "MEL",
                         ArrivalCity = "QLD",
                          StartTime = new TimeSpan(6, 0, 0),
                           EndTime  = new TimeSpan(12, 0, 0)
                }
            };
            
            context.Flights.AddRange(fligts);
            context.SaveChanges();
        }

        private static void SeedPassengerData(this FlightInfoContext context)
        {  
            var passengers = new List<Passenger>()
            {
                new Passenger()
                {
                   Name = "Tim",
                    Mobile = "0420 123 456"
                },
                new Passenger()
                {                  
                   Name = "Michelle",
                    Mobile = "0876 123 456"
                }
              };

            context.Passengers.AddRange(passengers);
            context.SaveChanges();
        }

        //private static void SeedBookingsData(this FlightInfoContext context)
        //{
        //    var bookings = new List<FlightBooking>();
        //       bookings.Add(new FlightBooking()
        //       {
        //           Flight = new Flight() {  Id=1},
        //           Passenger =  new Passenger() {  Id =1},             
        //           ArrivalCity = "MEL",
        //           DeparterCity = "SYD",
        //           Date = new DateTime(2017, 1, 10),
        //           NumberOfPassengers = 10
        //       });

        //    context.FlightBookings.AddRange(bookings);
        //    context.SaveChanges();
        //}

        public static void EnsureSeedDataForContext(this FlightInfoContext context)
        {
            if (!context.Flights.Any())
            {
                SeedFlightData(context);
            }

            if (!context.Passengers.Any())
            {
                SeedPassengerData(context);
            }

            //if (!context.FlightBookings.Any())
            //{
            //    SeedBookingsData(context);
            //}

        }
    }
}
