using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CarDealer.DTO
{
    internal class CarInfoDto
    {
        public string Make { get; set; }

        public string Model { get; set; }

        public long TravelledDistance { get; set; }
    }
}
