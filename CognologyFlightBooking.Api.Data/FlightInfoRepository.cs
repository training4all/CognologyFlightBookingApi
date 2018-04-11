using CognologyFlightBooking.Api.Data.Interfaces;
using CognologyFlightBooking.Api.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace CognologyFlightBooking.Api.Data
{
    public class FlightBooking1
    {

        public string FlightId;

        public string PassengerId;

        public string ArrivalCity;

        public string DeparterCity;

        public DateTime Date;

        public int NoOfPassenger;

    }

    public class Passenger1
    {

        public string PassengerId;

        public string Name;

        public string Mobile;

    }

    public class Flight1
    {

        public string FlightId;

        public string FlightNumber;

        public int PassengerCapacity;

    }

    public class FlightInfoRepository : IFlightInfoRepository
    {
        private FlightInfoContext _context;

        public FlightInfoRepository(FlightInfoContext context)
        {
            _context = context;
        }

        public IEnumerable<Flight> GetFlights()
        {
            return _context.Flights.ToList<Flight>();
        }

        public Flight GetFlightByFlightNumber(string flightNumber)
        {
            return _context.Flights.Where(f => f.FlightNumber == flightNumber).FirstOrDefault<Flight>();
        }

        public IEnumerable<Passenger> GetPassengers()
        {
            return _context.Passengers.ToList<Passenger>();
        }

        public IEnumerable<Passenger> GetPassengersByName(string name)
        {
            return _context.Passengers.Where(p => p.Name == name).ToList<Passenger>();
        }

        public Passenger GetPassengersByNameAndMobile(string name, string mobile)
        {
            return _context.Passengers.Where(p => p.Name == name && p.Mobile == mobile).FirstOrDefault<Passenger>();
        }

        public Passenger GetPassengerById(int id)
        {
            return _context.Passengers.Where(p => p.Id == id).FirstOrDefault<Passenger>();
        }

        public void AddPassenger(Passenger passenger)
        {
            _context.Passengers.Add(passenger);                     
        }

        public void AddFlight(Flight flight)
        {
            _context.Flights.Add(flight);
        }

     
        public FlightBooking GetFlightBookingById(int id)
        {
            //return _context.FlightBookings.Where(b => b.Id == id).include.FirstOrDefault<FlightBooking>();
            return _context.FlightBookings.Include(b => b.Flight).Include(b => b.Passenger)
                .Where(b => b.Id == id).FirstOrDefault<FlightBooking>();

        }

        public void MakeBooking(FlightBooking booking)
        {
            _context.FlightBookings.Add(booking);
        }

        public IEnumerable<FlightBooking> GetBookings()
        {
            return _context.FlightBookings.Include(b => b.Flight).Include(b => b.Passenger).ToList<FlightBooking>();
        }

        private void CheckCOde()
        {

            List<FlightBooking1> bookings = new List<FlightBooking1>();

            bookings.Add(new FlightBooking1()
            {

                FlightId = "1",

                PassengerId = "P1",

                ArrivalCity = "MEL",

                DeparterCity = "SYD",

                Date = new DateTime(2017, 1, 10),

                NoOfPassenger = 10

            });

            bookings.Add(new FlightBooking1()
            {

                FlightId = "1",

                PassengerId = "P2",

                ArrivalCity = "SYD",

                DeparterCity = "MEL",

                Date = new DateTime(2017, 1, 02),

                NoOfPassenger = 10

            });

            bookings.Add(new FlightBooking1()
            {

                FlightId = "1",

                PassengerId = "P2",

                ArrivalCity = "MEL",

                DeparterCity = "SYD",

                Date = new DateTime(2017, 1, 08),

                NoOfPassenger = 10

            });

            bookings.Add(new FlightBooking1()
            {

                FlightId = "2",

                PassengerId = "P3",

                ArrivalCity = "MEL",

                DeparterCity = "SYD",

                Date = new DateTime(2017, 1, 07),

                NoOfPassenger = 20

            });

            List<Flight1> flights = new List<Flight1>();

            flights.Add(new Flight1()
            {

                FlightId = "1",

                FlightNumber = "F100",

                PassengerCapacity = 23

            });

            flights.Add(new Flight1()
            {

                FlightId = "2",

                FlightNumber = "F200",

                PassengerCapacity = 20

            });

            flights.Add(new Flight1()
            {

                FlightId = "3",

                FlightNumber = "F300",

                PassengerCapacity = 20

            });



            List<Passenger1> passengers = new List<Passenger1>();

            passengers.Add(new Passenger1()
            {

                PassengerId = "P1",

                Name = "Sui",

                Mobile = "1233344"

            });

            passengers.Add(new Passenger1()
            {

                PassengerId = "P2",

                Name = "Ananda",

                Mobile = "1233344"

            });

            var arrivalCity = "MEL";

            var departerCity = "SYD";

            var bookingDate = new DateTime(2017, 1, 10);

            var passengerName = "Sui";

            var flightNumber = "F100";
            var SearchedBookings =

            from FB in bookings

            where FB.ArrivalCity == arrivalCity && FB.DeparterCity == departerCity && FB.Date == bookingDate

                                    && FB.PassengerId == ((from P in passengers

                                                           where P.Name == passengerName

                                                           select P.PassengerId).FirstOrDefault())

                                    && FB.FlightId == ((from F in flights

                                                        where F.FlightNumber == flightNumber

                                                        select F.FlightId).FirstOrDefault())

            select FB;
            foreach (var SB in SearchedBookings)

            {

                Console.WriteLine("{0}, {1}, {2}, {3}", SB.FlightId, SB.PassengerId, SB.ArrivalCity, SB.DeparterCity);

            }
        }

        public IEnumerable<FlightBooking> SearchBooking(string passengerName, string flightNumber, string arrivalCity,
                                            string departerCity, DateTime bookingDate)
        {
           var SearchedBookings = from FB in GetBookings()
                                   where FB.ArrivalCity == arrivalCity && FB.DeparterCity == departerCity && FB.Date == bookingDate
                                    && FB.Passenger.Id == ((from P in _context.Passengers
                                                            where P.Name == passengerName
                                                            select P.Id).FirstOrDefault())
                                    && FB.Flight.Id == ((from F in _context.Flights
                                                         where F.FlightNumber == flightNumber
                                                         select F.Id).FirstOrDefault())
                                   select FB;

           // IEnumerable<FlightBooking> b = Enumerable.Empty<FlightBooking>();
            return SearchedBookings;
        }

        public IEnumerable<Flight> CheckAvailability(int noOfPassengers, DateTime startDate, DateTime endDate)
        {
            //group flight booking entity which passes startDate and endDate on flight and sum of passengers booked for that flight
            var FBookingsGrouped = from FB in GetBookings()
                                   where FB.Date < startDate || FB.Date > endDate
                                   group FB by FB.Flight.Id into FBg
                                   select new {
                                       FlightId = FBg.Key,
                                       TotalNoOfPassenger = FBg.Sum(FB => FB.NumberOfPassengers)
                                   };

            //Available flights are flights which are in Flight entity but not in Flight Bookings Entity
            // OR flights which are booked but their passenger capacity is still enough to accomodate new noOfPassengers
            var availableFlights = from F in _context.Flights
                                  where !(from FB in FBookingsGrouped  select FB.FlightId).Contains(F.Id)
                                  ||
                                 (F.PassengerCapacity - ((from FB in FBookingsGrouped 
                                                            where FB.FlightId == F.Id
                                                            select FB.TotalNoOfPassenger).FirstOrDefault()) >= noOfPassengers)
                                 select F;
            return availableFlights;
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
