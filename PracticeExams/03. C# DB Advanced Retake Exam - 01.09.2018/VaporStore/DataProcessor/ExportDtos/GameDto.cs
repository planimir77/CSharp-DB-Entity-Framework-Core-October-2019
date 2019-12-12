using System.Xml.Serialization;

namespace VaporStore.DataProcessor.ExportDtos
{
    [XmlType("Game")]
    public class GameDto
    {
        [XmlAttribute(AttributeName="title")]
        public string Title { get; set; }

        [XmlElement(ElementName="Genre")]
        public string Genre { get; set; }

        [XmlElement(ElementName="Price")]
        public decimal Price { get; set; }

    }
}