using System.Xml.Serialization;
using Newtonsoft.Json;

namespace TeisterMask.DataProcessor.ExportDto
{
    [XmlType("Task")]
    public  class TaskExportDto
    {
        [XmlElement(ElementName="Name")]
        [JsonProperty("TaskName")]
        public string TaskName { get; set; }

        [JsonProperty("OpenDate")]
        public string OpenDate { get; set; }

        [JsonProperty("DueDate")]
        public string DueDate { get; set; }

        [XmlElement(ElementName="Label")]
        [JsonProperty("LabelType")]
        public string LabelType { get; set; }

        [JsonProperty("ExecutionType")]
        public string ExecutionType { get; set; }
    }
}
