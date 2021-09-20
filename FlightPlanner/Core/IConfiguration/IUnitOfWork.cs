using System.Threading.Tasks;
using FlightPlanner.Core.IRepositories;

namespace FlightPlanner.Core.IConfiguration
{
    public interface IUnitOfWork
    {
        IFlightRepository Flights { get; }

        Task CompleteAsync();
    }
}