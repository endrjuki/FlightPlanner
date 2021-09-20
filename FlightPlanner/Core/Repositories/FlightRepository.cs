using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightPlanner.Core.IRepositories;
using FlightPlanner.Data;
using FlightPlanner.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightPlanner.Core.Repositories
{
    public class FlightRepository : GenericRepository<Flight>, IFlightRepository
    {
        public FlightRepository(ApplicationDbContext context, ILogger logger)
            : base(context, logger)
        {

        }

        public override async Task<IEnumerable<Flight>> All()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "{Repo} All method error", typeof(FlightRepository));
                return new List<Flight>();
            }
        }

        public override async Task<bool> Upsert(Flight entity)
        {
            try
            {
                var existingFlight = await _dbSet.Where(f => f.Equals(entity))
                    .FirstOrDefaultAsync();
                if (existingFlight is null)
                {
                    return await Add(entity);
                }

                existingFlight.Carrier = entity.Carrier;
                existingFlight.From = entity.From;
                existingFlight.Id = entity.Id;
                existingFlight.To = entity.To;
                existingFlight.ArrivalTime = entity.ArrivalTime;
                existingFlight.DepartureTime = entity.DepartureTime;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Upsert method error", typeof(FlightRepository));
                return false;
            }
        }

        public async Task<bool> Exists(Flight flight)
        {
            try
            {
                var existingFlight = await _dbSet.Where(f => f.Equals(flight))
                    .FirstOrDefaultAsync();

                if (existingFlight is null)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Exists method error", typeof(FlightRepository));
                return false;
            }

        }

        public override async Task<bool> Delete(Guid id)
        {
            try
            {
                var existingFlight = await _dbSet.Where(f => f.Id == id)
                    .FirstOrDefaultAsync();

                if (existingFlight is null)
                {
                    return false;
                }

                _dbSet.Remove(existingFlight);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Delete method error", typeof(FlightRepository));
                return false;
            }
        }
    }
}