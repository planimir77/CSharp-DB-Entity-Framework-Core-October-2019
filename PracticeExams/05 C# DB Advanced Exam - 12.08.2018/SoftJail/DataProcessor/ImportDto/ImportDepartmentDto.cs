using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using SoftJail.Data.Models;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportDepartmentDto
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Cells")]
        public List<ImportCellDto> Cells { get; set; }
    }
}
