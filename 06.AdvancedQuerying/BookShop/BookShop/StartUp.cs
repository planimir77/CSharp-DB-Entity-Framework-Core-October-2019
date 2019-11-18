using System.Linq;
using System.Text;
using BookShop.Initializer;
using BookShop.Models.Enums;
using AutoMapper;
using BookShop.Models;

namespace BookShop
{
    using System;
    using Data;

    public class StartUp
    {
        public static void Main()
        {

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Book, BookDto>();
            });

            using (var db = new BookShopContext())
            {
                //DbInitializer.ResetDatabase(db);
                var book = db.Books.First();

                var bookDto = Mapper.Map<BookDto>(book);

            }

        }

        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(book => book.Copies < 4200)
                .ToArray();
            var booksToRemove = books.Length;
            foreach (var book in books)
            {
                var booksCategories = context.BooksCategories
                    .Where(category => category.BookId == book.BookId)
                    .ToArray();
                context.BooksCategories.RemoveRange(booksCategories);
                context.Books.Remove(book);
            }

            context.SaveChanges();

            return booksToRemove;
        }

        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                .Where(book => book.ReleaseDate.Value.Year < 2010)
                .ToArray();
            foreach (var book in books)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var recentBooks = context
                .Categories
                .Select(category => new
                {
                    CategoryName = category.Name,
                    Books = category.CategoryBooks
                        .Select(bookCategory => new
                        {
                            Title = bookCategory.Book.Title,
                            ReleaseDate = bookCategory.Book.ReleaseDate.Value
                        })
                        .OrderByDescending(arg => arg.ReleaseDate)
                        .Take(3)
                })
                .OrderBy(arg => arg.CategoryName)
                .ToArray();
            var result = new StringBuilder();

            foreach (var category in recentBooks)
            {
                result.AppendLine($"--{category.CategoryName}");
                
                foreach (var book in category.Books)
                {
                    result.AppendLine($"{book.Title} ({book.ReleaseDate.Year})");
                }
            }
            return result.ToString().TrimEnd();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var books = context
                .Categories
                .Select(category => new
                {
                    Category = category.Name,
                    Profit = category.CategoryBooks.Sum(bookCategory =>
                        bookCategory.Book.Price * bookCategory.Book.Copies)
                })
                .OrderByDescending(arg => arg.Profit)
                .ThenBy(arg => arg.Category)
                .ToList();

            return String.Join(Environment.NewLine, books.Select(arg => $"{arg.Category} ${arg.Profit:F2}"));
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context.Authors
                .Select(author => new
                {
                    FullName = author.FirstName + " " + author.LastName,
                    BooksCopies = author.Books
                        .Sum(book => book.Copies)
                })
                .OrderByDescending(arg => arg.BooksCopies)
                .ToList();

            return String.Join(Environment.NewLine, authors.Select(arg => $"{arg.FullName} - {arg.BooksCopies}"));
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var booksCount = context.Books
                .Count(book => book.Title.Length > lengthCheck);

            return booksCount;
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(book => book.Author.LastName.ToLower()
                    .StartsWith(input.ToLower()))
                .OrderBy(book => book.BookId)
                .Select(book => new
                {
                    book.Title,
                    FullName = new
                    {
                        AuthorFullName = book.Author.FirstName + " " + book.Author.LastName
                    }
                })
                .ToList();

            return String.Join(Environment.NewLine, books.Select(arg => $"{arg.Title} ({arg.FullName.AuthorFullName})"));
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var titles = context.Books
                .Where(book => book.Title.ToLower().Contains(input.ToLower()))
                .Select(book => book.Title)
                .OrderBy(s => s)
                .ToList();
            return String.Join(Environment.NewLine, titles);
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Where(x => x.FirstName.EndsWith(input))
                .Select(a => new
                {
                    FullName = a.FirstName + " " + a.LastName
                })
                .OrderBy(n => n.FullName)
                .ToList();

            return String.Join(Environment.NewLine, authors.Select(a => a.FullName));
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string inputDate)
        {
            DateTime date =
                DateTime.ParseExact(inputDate, "dd-MM-yyyy", null);

            var books = context
                .Books
                .Where(x => x.ReleaseDate.Value < date)
                .OrderByDescending(x => x.ReleaseDate)
                .Select(x => new
                {
                    Title = x.Title,
                    Edition = x.EditionType,
                    Price = x.Price
                })
                .ToList();

            return String.Join(Environment.NewLine,
                books.Select(x => $"{x.Title} - {x.Edition} - ${x.Price:F2}"));
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var category = input
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.ToLower())
                .ToArray();

            var books = context
                .Books
                .Where(b => b.BookCategories
                    .Any(c => category
                        .Contains(c.Category.Name.ToLower())))
                .OrderBy(t => t.Title)
                .Select(t => t.Title)
                .ToList();

            return String.Join(Environment.NewLine, books);
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context
                .Books
                .Where(x => x.ReleaseDate.Value.Year != year)
                .OrderBy(x => x.BookId)
                .Select(x => x.Title)
                .ToList();

            return String.Join(Environment.NewLine, books);
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var result = new StringBuilder();

            var titleAndPrice = context
                .Books
                .Where(x => x.Price > 40)
                .OrderByDescending(x => x.Price)
                .Select(x => new
                {
                    Title = x.Title,
                    Price = x.Price
                })
                .ToList();

            foreach (var item in titleAndPrice)
            {
                result.AppendLine($"{item.Title} - ${item.Price:F2}");
            }

            //return String.Join(Environment.NewLine,titleAndPrice.Select(t=> $"{t.Title} - ${t.Price:F2}"));
            return result.ToString().Trim();
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var result = new StringBuilder();

            var booksTitle = context
                .Books
                .Where(x => x.EditionType == EditionType.Gold && x.Copies < 5000)
                .OrderBy(x => x.BookId)
                .Select(x => x.Title)
                .ToArray();

            foreach (var title in booksTitle)
            {
                result.AppendLine(title);
            }

            return result.ToString().TrimEnd();
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var titles = context.Books
                .Where(x => x.AgeRestriction.ToString().ToLower() == command.ToLower())
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToArray();
            var result = new StringBuilder();

            foreach (var title in titles)
            {
                result.AppendLine(title);
            }
            return result.ToString().TrimEnd();
        }
    }
}
