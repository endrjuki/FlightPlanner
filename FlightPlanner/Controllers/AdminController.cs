using System;
using System.Threading.Tasks;
using FlightPlanner.Core.IConfiguration;
using FlightPlanner.Models;
using FlightPlanner.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FlightPlanner.Controllers
{
    [Authorize]
    [Route("admin-api")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IUnitOfWork _unitOfWork;


        public AdminController(ILogger<AdminController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("flights/{id}")]
        public async Task<IActionResult> GetFlight(Guid id)
        {
            var flight = await _unitOfWork.Flights.GetById(id);

            if (flight is null)
            {
                return NotFound();
            }

            return Ok(flight);
        }

        [HttpPut]
        [Route("flights")]
        public async Task<IActionResult> PutFlight(Flight flight)
        {
            if (await _unitOfWork.Flights.Exists(flight))
            {
                return Conflict();
            }

            if (flight.IsValid())
            {
                flight.Id = Guid.NewGuid();
                await _unitOfWork.Flights.Add(flight);
                await _unitOfWork.CompleteAsync();

                return Created("", flight);
            }

            return BadRequest();
        }

        [HttpDelete]
        [Route("flights/{id}")]
        public IActionResult DeleteFlight(Guid id)
        {
            _unitOfWork.Flights.Delete(id);
            return Ok();
        }
    }
}