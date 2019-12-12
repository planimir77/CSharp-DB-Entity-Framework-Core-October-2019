using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace VaporStore.DataProcessor.ImportDtos
{
    [XmlType("Purchase")]
    public class ImportPurchasesDto
    {
        [Required] 
        [XmlElement(ElementName = "Type")]
        public string Type { get; set; }

        [Required]
        [RegularExpression("^[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}$")]
        [XmlElement(ElementName = "Key")]
        public string Key { get; set; }

        [Required] 
        [XmlElement(ElementName = "Card")]
        public string Card { get; set; }

        [Required] 
        [XmlElement(ElementName = "Date")]
        public string Date { get; set; }

        [Required] 
        [XmlAttribute(AttributeName = "title")]
        public string Title { get; set; }
    }
}
