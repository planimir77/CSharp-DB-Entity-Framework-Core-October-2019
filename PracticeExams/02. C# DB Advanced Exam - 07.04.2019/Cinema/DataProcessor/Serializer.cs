using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Cinema.Data;
using Cinema.DataProcessor.ExportDto;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace Cinema.DataProcessor
{
    using System;

    using Data.Models;

    public class Serializer
    {
        public static string ExportTopMovies(CinemaContext context, int rating)
        {
            var topMoviesJson = context.Movies
                .Where(m => m.Rating >= rating && m.Projections.Any(p => p.Tickets.Count >= 1))
                .OrderByDescending(m => m.Rating)
                .ThenByDescending(m => m.Projections.Sum(p => p.Tickets.Sum(t => t.Price)))
                .Select(m => new ExportTopMoviesDto
                {
                    MovieName = m.Title,
                    Rating = m.Rating.ToString("F2"),
                    TotalIncomes = m.Projections.Sum(p => p.Tickets.Sum(t => t.Price)).ToString("F2"),
                    Customers = m.Projections
                        .SelectMany(p => p.Tickets)
                        .Select(c => new CustomerDto
                        {
                            FirstName = c.Customer.FirstName,
                            LastName = c.Customer.LastName,
                            Balance = c.Customer.Balance.ToString("F2")
                        })
                        .OrderByDescending(c => c.Balance)
                        .ThenBy(c => c.FirstName)
                        .ThenBy(c => c.LastName)
                        .ToList()
                })
                .Take(10)
                .ToList();

            var result = JsonConvert.SerializeObject(topMoviesJson, Formatting.Indented);

            return result;
        }

        public static string ExportTopCustomers(CinemaContext context, int age)
        {
            var customers = context.Customers
                .Where(c => c.Age >= age)
                .OrderByDescending(c => c.Tickets.Sum(ticket => ticket.Price))
                .Select(customer => new ExportTopCustomerDto
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    SpentMoney = customer.Tickets.
                        Sum(ticket => ticket.Price).ToString("F"),
                    SpentTime = TimeSpan.FromMilliseconds
                        (
                            customer.Tickets
                                .Sum(ticket => ticket.Projection.Movie.Duration.TotalMilliseconds)
                        )
                        .ToString(@"hh\:mm\:ss")

                })
                .Take(10)
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportTopCustomerDto[]),
                new XmlRootAttribute("Customers"));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, customers, namespaces);
            }

            return sb.ToString().TrimEnd();
        }
    }
}