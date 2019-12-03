using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace MusicHub.DataProcessor.ImportDtos
{
    [XmlType("Performer")]
    public class ImportSongPerformerDto
    {
        [MinLength(3),MaxLength(20), Required]
        [XmlElement(ElementName="FirstName")]
        public string FirstName { get; set; }

        [MinLength(3),MaxLength(20), Required]
        [XmlElement(ElementName="LastName")]
        public string LastName { get; set; }

        [Range(18, 70), Required]
        [XmlElement(ElementName="Age")]
        public int Age { get; set; }

        [Range(0, double.MaxValue), Required]
        [XmlElement(ElementName="NetWorth")]
        public decimal NetWorth { get; set; }

        [XmlArray(ElementName = "PerformersSongs")]
        public List<PerformersSongsDto> PerformersSongs { get; set; }
    }

    [XmlType("Song")]
    public class PerformersSongsDto 
    {
        [XmlAttribute(AttributeName="id")]
        public int Id { get; set; }
    }
}
