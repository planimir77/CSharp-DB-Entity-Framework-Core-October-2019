using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Cinema.DataProcessor.ExportDto
{
    [XmlType("Customer")]
    public class ExportTopCustomerDto
    {
        [XmlAttribute(AttributeName="FirstName")]
        public string FirstName { get; set; }

        [XmlAttribute(AttributeName="LastName")]
        public string LastName { get; set; }

        [XmlElement(ElementName="SpentMoney")]
        public string SpentMoney { get; set; }

        [XmlElement(ElementName="SpentTime")]
        public string SpentTime { get; set; }


    }
}
