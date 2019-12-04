using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Cinema.DataProcessor.ImportDto
{
    public class ImportHallSeatDto
    {
        public string Name { get; set; }

        [JsonProperty("Is4Dx")]
        public bool Is4Dx { get; set; }

        [JsonProperty("Is3D")]
        public bool Is3D { get; set; }

        public int Seats { get; set; }
    }
}
