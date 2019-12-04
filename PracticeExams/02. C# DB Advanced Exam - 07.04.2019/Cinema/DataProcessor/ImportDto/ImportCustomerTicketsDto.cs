using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Cinema.DataProcessor.ImportDto
{
    [XmlType("Customer")]
    public class ImportCustomerTicketsDto
    {
        [XmlElement(ElementName="FirstName")]
        public string FirstName { get; set; }

        [XmlElement(ElementName="LastName")]
        public string LastName { get; set; }

        [XmlElement(ElementName="Age")]
        public string Age { get; set; }

        [XmlElement(ElementName="Balance")]
        public string Balance { get; set; }

        [XmlArray(ElementName="Tickets")]
        public List<TicketDto> Tickets { get; set; }
    }
    [XmlType("Ticket")]
    public class TicketDto {
        [XmlElement(ElementName="ProjectionId")]
        public string ProjectionId { get; set; }
        [XmlElement(ElementName="Price")]
        public string Price { get; set; }
    }
}
