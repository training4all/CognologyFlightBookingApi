using Microsoft.AspNetCore.Mvc;
using CognologyFlightBooking.Api.Data.Interfaces;
using System.Collections.Generic;
using CognologyFlightBooking.Api.Data.Entities;
using CognologyFlightBooking.Api.Models;
using Microsoft.Extensions.Logging;
using System;

namespace CognologyFlightBooking.Api.Controllers
{
    [Route("api/passengers")]
    public class PassengerController : Controller
    {
        private ILogger<PassengerController> _logger;
        private IFlightInfoRepository _flightInfoRepository;

        public PassengerController(IFlightInfoRepository flightInfoRepository, ILogger<PassengerController> logger)
        {
            _flightInfoRepository = flightInfoRepository;
            _logger = logger;
        }

        [HttpGet()]
        public IActionResult GetPassengers()
        {
            try
            {
                _logger.LogInformation("GBMP- start logging");
                var passengerEntities = _flightInfoRepository.GetPassengers();
                var results = AutoMapper.Mapper.Map<IEnumerable<PassengerDto>>(passengerEntities);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("An Error Occured While Handing Your Request. {0}", ex.ToString());
                return StatusCode(500, "An Error Occured While Handing Your Request.");
            }

        }

        [HttpGet("{passengerName}/{mobile}", Name = "GetPassengersByNameAndMobile")]
        public IActionResult GetPassengersByNameAndMobile(string passengerName, string mobile)
        {  
            try
            {
                Passenger passengerEntities = _flightInfoRepository.GetPassengersByNameAndMobile(passengerName, mobile);
                var results = AutoMapper.Mapper.Map<PassengerDto>(passengerEntities);
                if (results == null)
                {
                    return NotFound();
                }
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("An Error Occured While Handing Your Request. {0}", ex.ToString());
                return StatusCode(500, "An Error Occured While Handing Your Request.");
            }
        }

        [HttpGet("{id}", Name = "GetPassengerById")]
        public IActionResult GetPassengerById(int id)
        {
            try
            {
                Passenger passengerEntity = _flightInfoRepository.GetPassengerById(id);
                var results = AutoMapper.Mapper.Map<PassengerDto>(passengerEntity);
                if (results == null)
                {
                    _logger.LogInformation($"Passenger not found with ID: {id}");
                    return NotFound();
                }
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("An Error Occured While Handing Your Request. {0}", ex.ToString());
                return StatusCode(500, "An Error Occured While Handing Your Request.");
            }
        }
                
        [HttpPost]
        public IActionResult CreatePassenger([FromBody] PassengerDto passenger)
        {
            try
            {
                if (passenger == null)
                {
                    return BadRequest();
                }

                // custom validation to make sure duplicate passengers don't get created
                if (_flightInfoRepository.GetPassengersByNameAndMobile(passenger.Name, passenger.Mobile) != null)
                {
                    ModelState.AddModelError("Passenger", "Passenger Already Exists.");
                    _logger.LogCritical($"Passenger Already Exists with Name: {passenger.Name}, mobile: {passenger.Mobile}");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var passengerEntity = AutoMapper.Mapper.Map<Passenger>(passenger);
                _flightInfoRepository.AddPassenger(passengerEntity);

                if (!_flightInfoRepository.Save())
                {
                    _logger.LogCritical("An Error Occured While Handing Your Request.");
                    return StatusCode(500, "An Error Occured While Handing Your Request.");
                }

                var newPassenger = AutoMapper.Mapper.Map<PassengerDto>(passengerEntity);
                return CreatedAtRoute("GetPassengerById", new { id = passengerEntity.Id }, newPassenger);
            }
            catch(Exception ex)
            {
                _logger.LogCritical("An Error Occured While Handing Your Request. {0}", ex.ToString());
                return StatusCode(500, "An Error Occured While Handing Your Request.");
            }
        }
    }
}
