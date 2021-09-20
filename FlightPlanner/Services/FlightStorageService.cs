using System;
using System.Collections.Generic;
using System.Linq;
using FlightPlanner.Models;

namespace FlightPlanner.Services
{
    public class FlightStorageService : IFlightStorageService
    {
        private readonly List<Flight> _flights;
        private int _flightId;

        public FlightStorageService()
        {
            _flights = new List<Flight>();
            _flightId = 1;
        }

        public Flight GetById(int id)
        {
            //return _flights.SingleOrDefault(flight => flight.Id == id);
            return new Flight();
        }

        public Airport[] GetAirportByKeyword(string keyword)
        {
            var cleanKeyword = keyword.Replace(" ", "").ToUpper();
            var airports = _flights
                .Select(a => a.From)
                .Where(a => a.AirportCode.ToUpper().StartsWith(cleanKeyword) ||
                            a.City.ToUpper().StartsWith(cleanKeyword) ||
                            a.Country.ToUpper().StartsWith(cleanKeyword));

            return airports.ToArray();
        }

        public void ClearFlights()
        {
            _flights.Clear();
        }

        public Flight AddFlight(Flight flight)
        {
            //flight.Id = _flightId;
            _flights.Add(flight);
            _flightId++;
            return flight;
        }

        public bool IsDuplicate(Flight flight)
        {
            if (_flights.Count == 0)
            {
                return false;
            }
            return _flights.ToArray().Last().Equals(flight);
        }

        public bool IsValid(Flight flight)
        {
            return flight != null && flight.IsValid();
        }

        public bool IsValidTimeframe(Flight flight)
        {
            var departureTime = DateTime.Parse(flight.DepartureTime);
            var arrivalTime = DateTime.Parse(flight.ArrivalTime);

            return departureTime < arrivalTime;
        }

        public void DeleteFlight(int id)
        {
            var flight = GetById(id);
            _flights.Remove(flight);
        }

        public SearchFlightResults SearchByParams(string from, string to, string date)
        {

            var filteredFlight = _flights.Where(f => f.From.AirportCode == from ||
                                                 f.To.AirportCode == to ||
                                                 f.DepartureTime == date).ToArray();

            return new SearchFlightResults(filteredFlight);
        }
    }
}