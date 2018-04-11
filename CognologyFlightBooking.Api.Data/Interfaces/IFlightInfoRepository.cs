using System.Collections;
using CognologyFlightBooking.Api.Data.Entities;
using System.Collections.Generic;
using System;

namespace CognologyFlightBooking.Api.Data.Interfaces
{
    public interface IFlightInfoRepository
    {
        IEnumerable<Flight> GetFlights();
        Flight GetFlightByFlightNumber(string flightNumber);
        IEnumerable<Passenger> GetPassengers();
        IEnumerable<Passenger> GetPassengersByName(string name);
        bool Save();
        void AddPassenger(Passenger passenger);
        Passenger GetPassengerById(int id);
        Passenger GetPassengersByNameAndMobile(string name, string mobile);
        void AddFlight(Flight flight);
        IEnumerable<FlightBooking> SearchBooking(string passengerName, string flightNumber, string arrivalCity,
                                                string departerCity, DateTime bookingDate);
        void MakeBooking(FlightBooking booking);
        FlightBooking GetFlightBookingById(int id);
        IEnumerable<FlightBooking> GetBookings();
        IEnumerable<Flight> CheckAvailability(int noOfPassengers, DateTime startDate, DateTime endDate);
    }
}
