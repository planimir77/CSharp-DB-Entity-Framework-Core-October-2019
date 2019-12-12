using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportCellDto
    {
        [JsonProperty("CellNumber")]
        public long CellNumber { get; set; }

        [JsonProperty("HasWindow")]
        public bool HasWindow { get; set; }
    }
}
