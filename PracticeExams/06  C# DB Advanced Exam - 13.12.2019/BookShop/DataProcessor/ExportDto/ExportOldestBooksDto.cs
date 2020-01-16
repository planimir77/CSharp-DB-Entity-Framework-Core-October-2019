using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace BookShop.DataProcessor.ExportDto
{
    [XmlType("Book ")]
    public class ExportOldestBooksDto
    {
        [XmlAttribute(AttributeName="Pages")]
        public string Pages { get; set; }

        [XmlElement(ElementName="Name")]
        public string Name { get; set; }

        [XmlElement(ElementName="Date")]
        public string Date { get; set; }

        
    }
}
