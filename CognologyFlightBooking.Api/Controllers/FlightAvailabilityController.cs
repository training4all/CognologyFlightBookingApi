using Microsoft.AspNetCore.Mvc;
using CognologyFlightBooking.Api.Data.Interfaces;
using System.Collections.Generic;
using CognologyFlightBooking.Api.Data.Entities;
using CognologyFlightBooking.Api.Models;
using Microsoft.Extensions.Logging;
using System;

namespace CognologyFlightBooking.Api.Controllers
{
    [Route("api/availableflights")]
    public class FlightAvailabilityController : Controller
    {
        private ILogger<FlightController> _logger;
        private IFlightInfoRepository _flightInfoRepository;


        public FlightAvailabilityController(IFlightInfoRepository flightInfoRepository, ILogger<FlightController> logger)
        {
            _flightInfoRepository = flightInfoRepository;
            _logger = logger;
        }

        [HttpGet("{noOfPassengers}")]
        public IActionResult CheckAvailability(int noOfPassengers, DateTime startDate, DateTime endDate)
        {
            IEnumerable<Flight> flights = _flightInfoRepository.CheckAvailability(noOfPassengers, startDate, endDate);

            if (flights == null)
            {
                _logger.LogInformation($"No Flights are available");
                return NotFound();
            }

            var results = AutoMapper.Mapper.Map<IEnumerable<FlightDto>>(flights);
            return Ok(results);
        }
    }
}
