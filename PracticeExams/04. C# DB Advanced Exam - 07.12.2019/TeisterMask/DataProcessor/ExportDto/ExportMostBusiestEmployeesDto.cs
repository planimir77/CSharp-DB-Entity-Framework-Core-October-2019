using Newtonsoft.Json;

namespace TeisterMask.DataProcessor.ExportDto
{
    public class ExportMostBusiestEmployeesDto
    {
        [JsonProperty("Username")]
        public string Username { get; set; }

        [JsonProperty("Tasks")]
        public TaskExportDto[] Tasks { get; set; }
    }
}
