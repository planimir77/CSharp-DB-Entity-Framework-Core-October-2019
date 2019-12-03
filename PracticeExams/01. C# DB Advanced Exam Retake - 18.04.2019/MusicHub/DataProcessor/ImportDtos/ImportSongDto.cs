using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Xml.Serialization;
using MusicHub.Data.Models;
using MusicHub.Data.Models.Enums;

namespace MusicHub.DataProcessor.ImportDtos
{
    [XmlType("Song")]
    public class ImportSongDto
    {
        [MinLength(3),MaxLength(20), Required]
        [XmlElement(ElementName="Name")]
        public string Name { get; set; }

        [Required]
        [XmlElement(ElementName="Duration")]
        public string Duration { get; set; }

        [Required]
        [XmlElement(ElementName="CreatedOn")]
        public string CreatedOn { get; set; }

        [Required]
        [XmlElement(ElementName="Genre")]
        public string Genre { get; set; }

        //[ForeignKey(nameof(Album))]
        [XmlElement(ElementName="AlbumId")]
        public int? AlbumId { get; set; }

        //[ForeignKey(nameof(Writer)), Required]
        [Required]
        [XmlElement(ElementName="WriterId")]
        public int WriterId { get; set; }

        [Range(0, double.MaxValue),Required]
        [XmlElement(ElementName="Price")]
        public decimal  Price { get; set; }
        
    }
}
