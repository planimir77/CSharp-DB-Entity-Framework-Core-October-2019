using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using CarDealer.Models;

namespace CarDealer.Dtos.Export
{
    [XmlType("car")]
    public class CarInfo
    {
        [XmlAttribute(AttributeName="make")]
        public string Make { get; set; }

        [XmlAttribute(AttributeName="model")]
        public string Model { get; set; }

        [XmlAttribute(AttributeName="travelled-distance")]
        public long TravelledDistance { get; set; }
    }

    [XmlType("sale")]
    public class ExportSalesWithAppliedDiscountDto 
    {
        [XmlElement(ElementName="car")]
        public CarInfo Car { get; set; }

        [XmlElement(ElementName="discount")]
        public decimal Discount { get; set; }

        [XmlElement(ElementName="customer-name")]
        public string CustomerName { get; set; }

        [XmlElement(ElementName="price")]
        public decimal Price { get; set; }

        [XmlElement(ElementName="price-with-discount")]
        public decimal PriceWithDiscount { get; set; }
    }
}
