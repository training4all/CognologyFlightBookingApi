using Microsoft.AspNetCore.Mvc;
using CognologyFlightBooking.Api.Data.Interfaces;
using System.Collections.Generic;
using CognologyFlightBooking.Api.Data.Entities;
using CognologyFlightBooking.Api.Models;
using Microsoft.Extensions.Logging;
using System;

namespace CognologyFlightBooking.Api.Controllers
{
    [Route("api/bookings")]
    public class FlightBookingController : Controller
    {
        private ILogger<FlightController> _logger;
        private IFlightInfoRepository _flightInfoRepository;

    
        public FlightBookingController(IFlightInfoRepository flightInfoRepository, ILogger<FlightController> logger)
        {
            _flightInfoRepository = flightInfoRepository;
            _logger = logger;
        }

        [HttpGet()]
        public IActionResult GetBookings()
        {
            var results = AutoMapper.Mapper.Map<IEnumerable<BookingDto>>(_flightInfoRepository.GetBookings());
            if(results == null)
            {
                return NotFound();
            }

            return Ok(results);
        }
        
        //[HttpGet("{passengerName}")]        
        //public IActionResult SearchBooking(string passengerName, string flightNumber, string arrivalCity,
        //                                    string departerCity, DateTime bookingDate)
        //{
        //    IEnumerable<FlightBooking> bookings = _flightInfoRepository.SearchBooking(passengerName, flightNumber, arrivalCity,
        //                                                        departerCity, bookingDate);

        //    if (bookings == null)
        //    {
        //        _logger.LogInformation($"No bookings found");
        //        return NotFound();
        //    }

        //    var results = AutoMapper.Mapper.Map<IEnumerable<BookingDto>>(bookings);            
        //    return Ok(results);
        //}

        //[HttpGet("{noOfPassengers}")]       
        //public IActionResult CheckAvailability(int noOfPassengers, DateTime startDate, DateTime endDate)
        //{
        //    IEnumerable<Flight> flights = _flightInfoRepository.CheckAvailability(noOfPassengers, startDate, endDate);

        //    if (flights == null)
        //    {
        //        _logger.LogInformation($"No Flights are available");
        //        return NotFound();
        //    }

        //    var results = AutoMapper.Mapper.Map<IEnumerable<FlightDto>>(flights);
        //    return Ok(results);
        //}

        [HttpGet("{bookingId}", Name = "GetBookingById")]        
        public IActionResult GetBookingById(int bookingId)
        {
            var results = AutoMapper.Mapper.Map<BookingDto>(_flightInfoRepository.GetFlightBookingById(bookingId));
            if (results == null)
            {
                return NotFound();
            }

            return Ok(results);
        }

        [HttpPost]
        public IActionResult MakeBooking([FromBody] MakeBookingDto booking)
        {
            try
            {
                if (booking == null)
                {
                    return BadRequest();
                }

                //if (_flightInfoRepository.GetFlightByFlightNumber(booking.FlightNumber) == null)
                if (_flightInfoRepository.GetFlightByFlightNumber(booking.FlightNumber) == null)
                {
                    ModelState.AddModelError("Flight", "Flight does not Exists.");
                    //_logger.LogCritical($"Flight does not Exists for flight Number : {0}", booking.FlightNumber);
                    _logger.LogCritical($"Flight does not Exists for flight Number : {0}", booking.FlightNumber);
                }

                if (_flightInfoRepository.GetPassengersByNameAndMobile(booking.Passenger.Name, booking.Passenger.Mobile) == null)
                {
                    ModelState.AddModelError("Passenge", "Passenger does not Exists.");
                    _logger.LogCritical($"Passenge does not Exists for Passenge Name: {0} and Mobile: {1}", booking.Passenger.Name, booking.Passenger.Mobile);
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var flightBookingEntity = AutoMapper.Mapper.Map<FlightBooking>(booking);
                DateTime bookingDate = new DateTime(booking.Date.Year, booking.Date.Month, booking.Date.Day, 0, 0, 0);
                flightBookingEntity.Date = bookingDate;

                flightBookingEntity.Flight = new Flight();
                //flightBookingEntity.Flight.Id = _flightInfoRepository.GetFlightByFlightNumber(booking.FlightNumber).Id;
                flightBookingEntity.Flight.Id = _flightInfoRepository.GetFlightByFlightNumber(booking.FlightNumber).Id;

                flightBookingEntity.Passenger = new Passenger();
                flightBookingEntity.Passenger.Id = _flightInfoRepository.GetPassengersByNameAndMobile(booking.Passenger.Name, booking.Passenger.Mobile).Id;

                _flightInfoRepository.MakeBooking(flightBookingEntity);

                if (!_flightInfoRepository.Save())
                {
                    _logger.LogCritical("An Error Occured While Handing Your Request.");
                    return StatusCode(500, "An Error Occured While Handing Your Request.");
                }

                var newBooking = AutoMapper.Mapper.Map<MakeBookingDto>(flightBookingEntity);
                return CreatedAtRoute("GetBookingById", new { bookingId = flightBookingEntity.Id }, newBooking);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("An Error Occured in MakeBooking: {0} ", ex.ToString());
                return StatusCode(500, "An Error Occured While Handing Your Request.");
            }            
        }


    }
}
