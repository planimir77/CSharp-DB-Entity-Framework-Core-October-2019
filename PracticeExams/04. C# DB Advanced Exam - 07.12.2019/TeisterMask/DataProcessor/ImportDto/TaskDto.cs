using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ImportDto
{
    [XmlType("Task")]
    public class TaskDto
    {
        [Required] 
        [MinLength(2), MaxLength(40) ]
        [XmlElement(ElementName = "Name")] 
        public string Name { get; set; }

        [Required]
        [XmlElement(ElementName = "OpenDate")] 
        public string OpenDate { get; set; }

        [Required]
        [XmlElement(ElementName = "DueDate")] 
        public string DueDate { get; set; }

        [Required]
        [XmlElement(ElementName = "ExecutionType")]
        public string ExecutionType { get; set; }

        [Required]
        [XmlElement(ElementName = "LabelType")]
        public string LabelType { get; set; }
    }
}
