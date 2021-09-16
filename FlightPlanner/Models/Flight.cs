using System;

namespace FlightPlanner.Models
{
    public class Flight
    {
        public int Id { get; set; }
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
                   !string.IsNullOrEmpty(ArrivalTime);
        }
        public bool Equals(Flight otherFlight)
        {
            return this.Carrier.Equals(otherFlight.Carrier) &&
                   this.From.Equals(otherFlight.From) &&
                   this.To.Equals(otherFlight.To) &&
                   this.DepartureTime.Equals(otherFlight.DepartureTime) &&
                   this.ArrivalTime.Equals(otherFlight.ArrivalTime);
        }
    }
}