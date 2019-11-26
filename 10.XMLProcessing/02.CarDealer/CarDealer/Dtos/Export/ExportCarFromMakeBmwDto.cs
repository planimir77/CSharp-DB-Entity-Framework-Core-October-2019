using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using CarDealer.Models;

namespace CarDealer.Dtos.Export
{
    [XmlType("car")]
    public class ExportCarFromMakeBmwDto 
    {
        [XmlAttribute(AttributeName="id")]
        public int Id { get; set; }
        [XmlAttribute(AttributeName="model")]
        public string Model { get; set; }
        [XmlAttribute(AttributeName="travelled-distance")]
        public long Travelleddistance { get; set; }
    }
}
