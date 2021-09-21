using System.Threading.Tasks;
using FlightPlanner.Models;

namespace FlightPlanner.Core.IRepositories
{
    public interface IFlightRepository : IGenericRepository<Flight>
    {
        Task<bool> Exists(Flight flight);
        Task<Airport[]> GetAirportsByKeyword(string keyword);
        Task<SearchFlightResults> SearchByParams(string from, string to, string date);
    }
}