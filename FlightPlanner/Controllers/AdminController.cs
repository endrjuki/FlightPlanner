using FlightPlanner.Models;
using FlightPlanner.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Authorize]
    [Route("admin-api")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IFlightStorageService _flightStorageService;

        public AdminController(IFlightStorageService flightStorageService)
        {
            _flightStorageService = flightStorageService;
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlight(int id)
        {
            lock (_flightStorageService)
            {
                var flight = _flightStorageService.GetById(id);
                if (flight is null)
                {
                    return NotFound();
                }
                return Ok(flight);
            }
        }

        [HttpPut]
        [Route("flights")]
        public IActionResult PutFlight(Flight flight)
        {
            lock (_flightStorageService)
            {
                if (_flightStorageService.IsDuplicate(flight))
                {
                    return Conflict();
                }

                if (!_flightStorageService.IsValid(flight) || !_flightStorageService.IsValidTimeframe(flight))
                {
                    return BadRequest();
                }

                _flightStorageService.AddFlight(flight);
                return Created("", flight);
            }
        }

        [HttpDelete]
        [Route("flights/{id}")]
        public IActionResult DeleteFlight(int id)
        {
            lock (_flightStorageService)
            {
                _flightStorageService.DeleteFlight(id);
                return Ok();
            }
        }
    }
}