using System;

namespace FlightPlanner.Models
{
    public class Flight
    {
        public Guid Id { get; set; }
        public Airport From { get; set; }
        public Airport To { get; set; }
        public string Carrier { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }

        public bool IsValid()
        {
            return From != null &&
                   From.IsValid() &&
                   To != null &&
                   To.IsValid() &&
                   !From.Equals(To) &&
                   !string.IsNullOrEmpty(Carrier) &&
                   !string.IsNullOrEmpty(DepartureTime) &&
                   !string.IsNullOrEmpty(ArrivalTime) &&
                   IsValidTimeFrame();
        }
        public bool Equals(Flight otherFlight)
        {
            return Carrier.Equals(otherFlight.Carrier) &&
                   From.Equals(otherFlight.From) &&
                   To.Equals(otherFlight.To) &&
                   DepartureTime.Equals(otherFlight.DepartureTime) &&
                   ArrivalTime.Equals(otherFlight.ArrivalTime);
        }

        private bool IsValidTimeFrame()
        {
            var departureTime = DateTime.Parse(DepartureTime);
            var arrivalTime = DateTime.Parse(ArrivalTime);

            return departureTime < arrivalTime;
        }
    }
}