using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.Dtos.Export
{
    [XmlType("customer")]
    public class ExportTotalSalesByCustomerDto
    {
        [XmlAttribute(AttributeName="full-name")]
        public string FullName { get; set; }

        [XmlAttribute(AttributeName="bought-cars")]
        public int BoughtCars { get; set; }

        [XmlAttribute(AttributeName="spent-money")]
        public decimal SpentMoney { get; set; }
    }
}
