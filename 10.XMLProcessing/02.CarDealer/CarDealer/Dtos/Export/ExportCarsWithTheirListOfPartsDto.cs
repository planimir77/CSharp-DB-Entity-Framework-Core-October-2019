using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.Dtos.Export
{
    [XmlType("car")]
    public class ExportCarsWithTheirListOfPartsDto 
    {
        [XmlElement(ElementName="parts")]
        public ExportPartsDto Parts { get; set; }

        [XmlAttribute(AttributeName="make")]
        public string Make { get; set; }

        [XmlAttribute(AttributeName="model")]
        public string Model { get; set; }

        [XmlAttribute(AttributeName="travelled-distance")]
        public long TravelledDistance { get; set; }
    }

    [XmlRoot(ElementName="parts")]
    public class ExportPartsDto
    {
        public ExportPartsDto()
        {
            this.Parts = new List<ExportPartDto>();
        }
        [XmlElement(ElementName="part")]
        public List<ExportPartDto> Parts { get; set; }
    }

    [XmlRoot(ElementName="part")]
    public class ExportPartDto 
    {
        [XmlAttribute(AttributeName="name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName="price")]
        public decimal Price { get; set; }
    }
}
