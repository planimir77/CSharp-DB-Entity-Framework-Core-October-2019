using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ExportDto
{
    [XmlType("Project")]
    public class ProjectExportDto
    {
        [XmlElement(ElementName="ProjectName")]
        public string ProjectName { get; set; }

        [XmlElement(ElementName="HasEndDate")]
        public string HasEndDate { get; set; }

        [XmlArray(ElementName="Tasks")] 
        public TaskExportDto[] Tasks { get; set; }

        [XmlAttribute(AttributeName="TasksCount")]
        public string TasksCount { get; set; }
    }
}
