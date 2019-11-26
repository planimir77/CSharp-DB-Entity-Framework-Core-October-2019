using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.Dtos.Import
{
    [XmlType("Customer")]
    public class ImportCustomerDto
    {
        [XmlElement(ElementName="name")]
        public string Name { get; set; }
        [XmlElement(ElementName="birthDate")]
        public string BirthDate { get; set; }
        [XmlElement(ElementName="isYoungDriver")]
        public string IsYoungDriver { get; set; }
    }
}
