using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace MusicHub.DataProcessor.ExportDtos
{
    [XmlType("Song")]
    public class ExportSongsAboveDurationDto {
        private string _performer;

        [XmlElement(ElementName="SongName")]
        public string SongName { get; set; }

        [XmlElement(ElementName="Writer")]
        public string Writer { get; set; }

        [XmlElement(ElementName = "Performer", IsNullable = false)]
        public string Performer
        {
            get { return _performer.Length < 3 ? null : _performer; }
            set { _performer = value; }
        }

        [XmlElement(ElementName="AlbumProducer")]
        public string AlbumProducer { get; set; }

        [XmlElement(ElementName="Duration")]
        public string Duration { get; set; }
    }
}
