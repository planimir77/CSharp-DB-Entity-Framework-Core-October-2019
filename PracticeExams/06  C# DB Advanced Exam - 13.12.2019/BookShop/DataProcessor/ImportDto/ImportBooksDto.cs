using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace BookShop.DataProcessor.ImportDto
{
    [XmlType("Book")]
    public class ImportBooksDto
    {
        [Required] 
        [MinLength(3), MaxLength(30)]
        [XmlElement(ElementName="Name")]
        public string Name { get; set; }

        [Required]
        [XmlElement(ElementName="Genre")]
        public string Genre { get; set; }

        [Range(0.01, double.MaxValue)]
        [XmlElement(ElementName="Price")]
        public decimal Price { get; set; }

        [Range(50, 5000)]
        [XmlElement(ElementName="Pages")]
        public int Pages { get; set; }

        [Required]
        [XmlElement(ElementName="PublishedOn")]
        public string PublishedOn { get; set; }
    }
}
