using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace VaporStore.DataProcessor.ExportDtos
{
    [XmlType("User")]
    public class ExportUserDto
    {
        [XmlAttribute(AttributeName="username")]
        public string Username { get; set; }

        [XmlArray(ElementName="Purchases")]
        public PurchaseDto[] Purchases { get; set; }

        [XmlElement(ElementName="TotalSpent")]
        public decimal TotalSpent { get; set; }
    }
}
