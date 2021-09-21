using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightPlanner.Core.IConfiguration;
using FlightPlanner.Models;
using FlightPlanner.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public CustomerController(ILogger<CustomerController> logger, IUnitOfWork unitOfWork)
        {
            _logger = _logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("airports")]
        public async Task<IActionResult> GetAirports(string search)
        {
           var airportArray = await _unitOfWork.Flights.GetAirportsByKeyword(search);

           return airportArray.Length == 0 ? Ok(search) : Ok(airportArray);
        }

        [HttpGet]
        [Route("flights/{id}")]
        public async Task<IActionResult> GetFlight(int id)
        {
            var flight = await _unitOfWork.Flights.GetById(id);
            return flight == null ? NotFound() : Ok(flight);
        }

        [HttpPost]
        [Route("flights/search/")]
        public async Task<IActionResult> PostSearchFlights(SearchFlightRequest searchFlightRequest)
        {
            if (searchFlightRequest.From == searchFlightRequest.To)
            {
                return BadRequest();
            }

            var searchResults = await _unitOfWork.Flights.SearchByParams(searchFlightRequest.From, searchFlightRequest.To,
                searchFlightRequest.Date);
            return Ok(searchResults);
        }
    }
}