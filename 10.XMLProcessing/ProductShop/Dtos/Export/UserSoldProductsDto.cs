using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using ProductShop.Models;

namespace ProductShop.Dtos.Export
{
    [XmlType("User")]
    public class UserSoldProductsDto
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlArray("soldProducts")]
        public SoldProductDto[] Products { get; set; }
    }
}
