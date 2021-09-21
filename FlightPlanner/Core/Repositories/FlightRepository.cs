using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        //private readonly SemaphoreSlim _lock= new SemaphoreSlim(1, 1);

        public FlightRepository(ApplicationDbContext context, ILogger logger)
            : base(context, logger, context.Flights)
        {

        }

        public override async Task<bool> Add(Flight flight)
        {
            //await _lock.WaitAsync();
            try
            {
                if (await Exists(flight))
                {
                    return false;
                }

                await _dbSet.AddAsync(flight);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Exists method error", typeof(FlightRepository));
                return false;
            }
            finally
            {
                //_lock.Release();
            }
        }
        public async Task<bool> Exists(Flight flight)
        {
            try
            {
                var existingFlight = await _dbSet
                    .Include(f => f.From)
                    .Include(f => f.To)
                    .FirstOrDefaultAsync(f =>
                        f.Carrier == flight.Carrier &&
                        f.ArrivalTime == flight.ArrivalTime &&
                        f.DepartureTime == flight.DepartureTime &&
                        f.From.AirportCode == flight.From.AirportCode &&
                        f.To.AirportCode == flight.To.AirportCode
                    );



                if (existingFlight is null)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Exists method error", typeof(FlightRepository));
                throw ex;
            }

        }

        public async Task<Airport[]> GetAirportsByKeyword(string keyword)
        {
            try
            {
                var cleanKeyword = keyword.Trim().ToLower();

                return await _dbSet.Select(f => f.From)
                    .Where(a => a.City.StartsWith(cleanKeyword) ||
                                a.Country.StartsWith(cleanKeyword) ||
                                a.AirportCode.StartsWith(cleanKeyword))
                    .ToArrayAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} GetAirportByKeyword method error", typeof(FlightRepository));
                return Array.Empty<Airport>();
            }
        }

        public async Task<SearchFlightResults> SearchByParams(string from, string to, string date)
        {

            var filteredFlight = await _dbSet.Where(f => f.From.AirportCode == from ||
                                                     f.To.AirportCode == to ||
                                                     f.DepartureTime == date).ToArrayAsync();

            return new SearchFlightResults(filteredFlight);
        }

        public override async Task<bool> Delete(int id)
        {
            //await _lock.WaitAsync();
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
            finally
            {
                //_lock.Release();
            }
        }

        public override async Task<Flight> GetById(int id)
        {
            try
            {
                return await _dbSet
                    .Include(f => f.From)
                    .Include(f => f.To)
                    .FirstOrDefaultAsync(f => f.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} GetById method error", typeof(FlightRepository));
                return null;
            }
        }
    }
}