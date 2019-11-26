using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.Dtos.Import
{
    [XmlRoot(ElementName="partId")]
    public class PartIdDto
    {
        [XmlAttribute(AttributeName="id")]
        public int Id { get; set; }
    }
}
