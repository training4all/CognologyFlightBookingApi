using Microsoft.AspNetCore.Mvc;
using CognologyFlightBooking.Api.Data.Interfaces;
using System.Collections.Generic;
using CognologyFlightBooking.Api.Data.Entities;
using CognologyFlightBooking.Api.Models;
using Microsoft.Extensions.Logging;

namespace CognologyFlightBooking.Api.Controllers
{
    [Route("api/flights")]
    public class FlightController : Controller
    {
        private ILogger<FlightController> _logger;
        private IFlightInfoRepository _flightInfoRepository;

        public FlightController(IFlightInfoRepository flightInfoRepository, ILogger<FlightController> logger)
        {
            _flightInfoRepository = flightInfoRepository;
            _logger = logger;
        }

        [HttpGet()]
        public IActionResult GetFlights()
        {
            var results = AutoMapper.Mapper.Map<IEnumerable<FlightDto>>(_flightInfoRepository.GetFlights());
            return Ok(results);
        }

        [HttpGet("{flightNumber}", Name = "GetFlightByFlightNumber")]
        public IActionResult GetFlightByFlightNumber(string flightNumber)
        {
            Flight flight = _flightInfoRepository.GetFlightByFlightNumber(flightNumber);
            var results = AutoMapper.Mapper.Map<FlightDto>(flight);

            if (flight == null)
            {
                _logger.LogInformation($"Flight not found with Flight Number: {flightNumber}");
                return NotFound();
            }

            return Ok(results);
        }

        [HttpPost]
        public IActionResult CreateFlight([FromBody] FlightDto flight)
        {
            if (flight == null)
            {
                return BadRequest();
            }

            // custom validation to make sure duplicate flights don't get created
            if (_flightInfoRepository.GetFlightByFlightNumber(flight.FlightNumber) != null)
            {
                _logger.LogCritical("Flight Already Exists");
                ModelState.AddModelError("Flight", "Flight Already Exists.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var flightEntity = AutoMapper.Mapper.Map<Flight>(flight);
            _flightInfoRepository.AddFlight(flightEntity);

            if (!_flightInfoRepository.Save())
            {
                _logger.LogCritical($"An Error Occured While saving flight with flight number: {flightEntity.FlightNumber}");
                return StatusCode(500, "An Error Occured While Handing Your Request.");
            }

            var newFlight = AutoMapper.Mapper.Map<FlightDto>(flightEntity);
            return CreatedAtRoute("GetFlightByFlightNumber", new { flightNumber = newFlight.FlightNumber }, newFlight);
        }
    }
}
