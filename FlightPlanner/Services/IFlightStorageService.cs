using System;
using FlightPlanner.Models;

namespace FlightPlanner.Services
{
    public interface IFlightStorageService
    {
        public Flight GetById(int id);

        public Airport[] GetAirportByKeyword(string keyword);

        public void ClearFlights();

        public Flight AddFlight(Flight flight);

        public bool IsDuplicate(Flight flight);

        public bool IsValid(Flight flight);

        public bool IsValidTimeframe(Flight flight);

        public void DeleteFlight(int id);

        public SearchFlightResults SearchByParams(string from, string to, string date);
    }
}