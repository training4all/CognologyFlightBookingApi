using Microsoft.AspNetCore.Mvc;
using CognologyFlightBooking.Api.Data.Interfaces;
using System.Collections.Generic;
using CognologyFlightBooking.Api.Data.Entities;
using CognologyFlightBooking.Api.Models;
using Microsoft.Extensions.Logging;
using System;

namespace CognologyFlightBooking.Api.Controllers
{
    [Route("api/searchbookings")]
    public class SearchBookingController : Controller
    {
        private ILogger<FlightController> _logger;
        private IFlightInfoRepository _flightInfoRepository;


        public SearchBookingController(IFlightInfoRepository flightInfoRepository, ILogger<FlightController> logger)
        {
            _flightInfoRepository = flightInfoRepository;
            _logger = logger;
        }

        [HttpGet("{passengerName}")]
        public IActionResult SearchBooking(string passengerName, string flightNumber, string arrivalCity,
                                           string departerCity, DateTime bookingDate)
        {
            IEnumerable<FlightBooking> bookings = _flightInfoRepository.SearchBooking(passengerName, flightNumber, arrivalCity,
                                                                departerCity, bookingDate);

            if (bookings == null)
            {
                _logger.LogInformation($"No bookings found");
                return NotFound();
            }

            var results = AutoMapper.Mapper.Map<IEnumerable<BookingDto>>(bookings);
            return Ok(results);
        }
    }
}
