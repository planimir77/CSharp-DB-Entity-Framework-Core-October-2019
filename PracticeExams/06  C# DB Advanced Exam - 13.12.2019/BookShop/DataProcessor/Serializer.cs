using BookShop.Data.Models.Enums;
using BookShop.DataProcessor.ExportDto;
using Microsoft.EntityFrameworkCore;

namespace BookShop.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportMostCraziestAuthors(BookShopContext context)
        {
            var authors = context
                .Authors
                .OrderByDescending(author => author.AuthorsBooks.Count)
                .ThenBy(author => new string($"{author.FirstName} {author.LastName}"))
                .Select(author => new ExportMostCraziestAuthorDto
                {
                    AuthorName = $"{author.FirstName} {author.LastName}",
                    Books = author.AuthorsBooks.Select(book => new BookExportDto
                    {
                        BookName = book.Book.Name,
                        BookPrice = book.Book.Price.ToString("F2")
                    })
                        .OrderByDescending(dto => Decimal.Parse(dto.BookPrice))
                        .ToList()
                })
                .ToList();

            var result = JsonConvert.SerializeObject(authors, Formatting.Indented);

            return result;
        }

        public static string ExportOldestBooks(BookShopContext context, DateTime date)
        {
            var items = context.Books
                .Where(book => book.PublishedOn < date && book.Genre == Genre.Science)
                .OrderByDescending(book => book.Pages)
                .ThenByDescending(book => book.PublishedOn)
                .Select(book => new ExportOldestBooksDto
                {
                    Pages = book.Pages.ToString(),
                    Name = book.Name,
                    Date = book.PublishedOn.ToString("d",CultureInfo.InvariantCulture),
                })
                .Take(10)
                .ToArray();
            var serializer = new XmlSerializer(typeof(ExportOldestBooksDto[]),
                new XmlRootAttribute("Books"));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, items, namespaces);
            }

            return sb.ToString().TrimEnd();
        }
    }
}