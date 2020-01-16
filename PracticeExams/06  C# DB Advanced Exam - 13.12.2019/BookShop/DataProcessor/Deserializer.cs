using BookShop.Data.Models;
using BookShop.Data.Models.Enums;
using BookShop.DataProcessor.ImportDto;

namespace BookShop.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedBook
            = "Successfully imported book {0} for {1:F2}.";

        private const string SuccessfullyImportedAuthor
            = "Successfully imported author - {0} with {1} books.";

        public static string ImportBooks(BookShopContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ImportBooksDto[]),
                new XmlRootAttribute("Books"));

            var reader = new StringReader(xmlString);

            var booksDto = (ImportBooksDto[])serializer.Deserialize(reader);
            
            var books = new List<Book>(); 

            var sb = new StringBuilder();

            foreach (var bookDto in booksDto)
            {
                bool isValidEnum = Enum.IsDefined(typeof(Genre), int.Parse(bookDto.Genre));

                if (!IsValid(bookDto) || !isValidEnum)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                // such as invalid name, genre, price, pages or published date
                var book = new Book
                {
                    Name = bookDto.Name,
                    Genre = Enum.Parse<Genre>(bookDto.Genre),
                    Price = bookDto.Price,
                    Pages = bookDto.Pages,
                    PublishedOn = DateTime.ParseExact(bookDto.PublishedOn, "MM/dd/yyyy", CultureInfo.InvariantCulture)
                };
                if (!IsValid(book))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                books.Add(book);
                sb.AppendLine(String.Format(SuccessfullyImportedBook,book.Name, book.Price));
            }

            context.Books.AddRange(books);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportAuthors(BookShopContext context, string jsonString)
        {
            var authorsDto = JsonConvert.DeserializeObject<ImportAuthorsDto[]>(jsonString);

            var sb = new StringBuilder();

            var authors = new List<Author>();

            var authorsBooks = new List<AuthorBook>();

            foreach (var authorDto in authorsDto)
            {
                //as invalid first name, last name, email or phone), do not import 
                var email = authors.FirstOrDefault(author1 => author1.Email == authorDto.Email);

                if (!IsValid(authorDto) || email != null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Author author = new Author
                {
                    FirstName = authorDto.FirstName,
                    LastName = authorDto.LastName,
                    Email = authorDto.Email,
                    Phone = authorDto.Phone
                };

                var tempAuthorsBooks = new List<AuthorBook>();
               
                foreach (var bookDto in authorDto.Books)
                {
                    if (bookDto.Id == null)
                    {
                        continue;
                    }
                    var tempBook = context.Books.Find(bookDto.Id);

                    if (tempBook == null)
                    {
                        continue;
                    }
                    var authorBook = new AuthorBook
                    {
                        Author = author,
                        Book = tempBook
                    };

                    tempAuthorsBooks.Add(authorBook);
                }

                if (tempAuthorsBooks.Count == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                authors.Add(author);
                authorsBooks.AddRange(tempAuthorsBooks);

                sb.AppendLine(String.Format(SuccessfullyImportedAuthor, author.FirstName + " " + author.LastName, tempAuthorsBooks.Count.ToString()));

            }

            context.Authors.AddRange(authors);

            context.AuthorsBooks.AddRange(authorsBooks);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}