using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightPlanner.Models;
using FlightPlanner.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IFlightStorageService _flightStorageService;
        public CustomerController(IFlightStorageService flightStorageService)
        {
            _flightStorageService = flightStorageService;
        }

        [HttpGet]
        [Route("airports")]
        public IActionResult GetAirports(string search)
        {
           var airportArray = _flightStorageService.GetAirportByKeyword(search);

           return airportArray.Length == 0 ? Ok(search) : Ok(airportArray);
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlight(int id)
        {
            var flight = _flightStorageService.GetById(id);
            return flight == null ? NotFound() : Ok(flight);
        }

        [HttpPost]
        [Route("flights/search/")]
        public IActionResult PostSearchFlights(SearchFlightRequest searchFlightRequest)
        {
            if (searchFlightRequest.From == searchFlightRequest.To)
            {
                return BadRequest();
            }

            var searchResults = _flightStorageService.SearchByParams(searchFlightRequest.From, searchFlightRequest.To,
                searchFlightRequest.Date);
            return Ok(searchResults);
        }
    }
}