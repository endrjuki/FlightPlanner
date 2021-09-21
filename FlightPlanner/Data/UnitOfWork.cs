using System;
using System.Threading;
using System.Threading.Tasks;
using FlightPlanner.Core.IConfiguration;
using FlightPlanner.Core.IRepositories;
using FlightPlanner.Core.Repositories;
using FlightPlanner.Models;
using Microsoft.Extensions.Logging;

namespace FlightPlanner.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly SemaphoreSlim _lock= new SemaphoreSlim(1, 1);
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public IFlightRepository Flights { get; private set; }

        public UnitOfWork(ApplicationDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("logs");
            Flights = new FlightRepository(_context, _logger);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}