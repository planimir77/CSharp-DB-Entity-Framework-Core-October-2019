using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.Dtos.Import
{
    [XmlRoot(ElementName="parts")]
    public class PartsDto
    {
        [XmlElement(ElementName="partId")]
        public List<PartIdDto> PartsId { get; set; }
    }
}
