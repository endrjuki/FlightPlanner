using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FlightPlanner.Models
{
    [ComplexType]
    public class Airport
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        [JsonPropertyName("airport")]
        public string AirportCode { get; set; }

        public bool Equals(Airport otherAirport)
        {
            var originAirportCode = AirportCode.Trim().ToLower();
            var destinationAirportCode = otherAirport.AirportCode.Trim().ToLower();
            return originAirportCode == destinationAirportCode;
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Country) &&
                   !string.IsNullOrEmpty(City) &&
                   !string.IsNullOrEmpty(AirportCode);
        }
    }
}