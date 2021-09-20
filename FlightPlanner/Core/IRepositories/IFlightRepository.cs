using System.Threading.Tasks;
using FlightPlanner.Models;

namespace FlightPlanner.Core.IRepositories
{
    public interface IFlightRepository : IGenericRepository<Flight>
    {
        Task<bool> Exists(Flight flight);
    }
}