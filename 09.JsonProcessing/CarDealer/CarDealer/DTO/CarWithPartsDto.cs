using System.Collections.Generic;
using Newtonsoft.Json;

namespace CarDealer.DTO
{
    internal class CarWithPartsDto
    {
        [JsonProperty("car")]
        public CarInfoDto Car { get; set; }


        [JsonProperty("parts")]
        public ICollection<PartInfoDto> Parts { get; set; }
    }
}