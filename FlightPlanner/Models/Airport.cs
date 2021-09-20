using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FlightPlanner.Models
{
    [ComplexType]
    public class Airport
    {
        public string Country { get; set; }
        public string City { get; set; }
        [Key]
        [JsonPropertyName("airport")]
        public string AirportCode { get; set; }

        public bool Equals(Airport otherAirport)
        {
            var originAirportCode = AirportCode.Replace(" ", "").ToLower();
            var destinationAirportCode = otherAirport.AirportCode.Replace(" ", "").ToLower();
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