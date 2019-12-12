using System.Xml.Serialization;

namespace VaporStore.DataProcessor.ExportDtos
{
    [XmlType("Purchase")]
    public class PurchaseDto
    {
        [XmlElement(ElementName="Card")]
        public string Card { get; set; }
        [XmlElement(ElementName="Cvc")]
        public string Cvc { get; set; }
        [XmlElement(ElementName="Date")]
        public string Date { get; set; }
        [XmlElement(ElementName="Game")]
        public GameDto Game { get; set; }

    }
}