using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.Dtos.Import
{
    [XmlType("Sale")]
    public class ImportSaleDto
    {
        [XmlElement(ElementName="carId")]
        public string CarId { get; set; }
        [XmlElement(ElementName="customerId")]
        public string CustomerId { get; set; }
        [XmlElement(ElementName="discount")]
        public string Discount { get; set; }
    }
}
