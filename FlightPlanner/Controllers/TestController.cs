using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightPlanner.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("testing-api")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IFlightStorageService _flightStorageService;
        public TestController(IFlightStorageService flightStorageService)
        {
            _flightStorageService = flightStorageService;
        }

        [Route("clear")]
        [HttpPost]
        public IActionResult Clear()
        {
            _flightStorageService.ClearFlights();
            return Ok();
        }
    }
}