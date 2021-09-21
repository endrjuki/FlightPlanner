using System;
using System.Threading;
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
        private static readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
        private readonly ILogger<AdminController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public AdminController(ILogger<AdminController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("flights/{id}")]
        public async Task<IActionResult> GetFlight(int id)
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
            await _lock.WaitAsync();
            try
            {
                if (!flight.IsValid())
                {
                    return BadRequest();
                }

                if (await _unitOfWork.Flights.Exists(flight))
                {
                    return Conflict(flight);
                }

                await _unitOfWork.Flights.Add(flight);
                await _unitOfWork.CompleteAsync();
                return Created("", flight);
            }
            finally
            {
                _lock.Release();
            }
        }

        [HttpDelete]
        [Route("flights/{id}")]
        public async Task<IActionResult> DeleteFlight(int id)
        {
            await _unitOfWork.Flights.Delete(id);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }
    }
}