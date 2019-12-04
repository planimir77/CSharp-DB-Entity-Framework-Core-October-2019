using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Cinema.DataProcessor.ImportDto
{
    [XmlType("Projection")]
    public class ImportProjectionDto
    {
        [XmlElement(ElementName="MovieId")]
        public int MovieId { get; set; }

        [XmlElement(ElementName="HallId")]
        public int HallId { get; set; }

        [XmlElement(ElementName="DateTime")]
        public string DateTime { get; set; }
    }
}
