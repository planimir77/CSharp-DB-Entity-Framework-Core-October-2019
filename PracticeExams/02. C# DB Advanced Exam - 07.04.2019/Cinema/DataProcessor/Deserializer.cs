using System.Collections.Generic;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using AutoMapper;
using Cinema.Data.Models;
using Cinema.Data.Models.Enums;
using Cinema.DataProcessor.ImportDto;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;
//using ValidationContext = AutoMapper.ValidationContext;

namespace Cinema.DataProcessor
{
    using System;

    using Data;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";
        private const string SuccessfulImportMovie
            = "Successfully imported {0} with genre {1} and rating {2}!";
        private const string SuccessfulImportHallSeat
            = "Successfully imported {0}({1}) with {2} seats!";
        private const string SuccessfulImportProjection
            = "Successfully imported projection {0} on {1}!";
        private const string SuccessfulImportCustomerTicket
            = "Successfully imported customer {0} {1} with bought tickets: {2}!";

        public static string ImportMovies(CinemaContext context, string jsonString)
        {
            var moviesDtos = JsonConvert.DeserializeObject<ImportMovieDto[]>(jsonString);

            var sb = new StringBuilder();

            var movies = new List<Movie>();

            foreach (var movieDto in moviesDtos)
            {
                Movie movie = Mapper.Map<Movie>(movieDto);

                bool isValidGenre = Enum.IsDefined(typeof(Genre), movie.Genre);
                var isExist = movies.Any(m => m.Title == movie.Title);

                if (!IsValid(movie) || !isValidGenre || isExist)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                movies.Add(movie);

                sb.AppendLine(String.Format(SuccessfulImportMovie, movie.Title, movie.Genre, $"{movie.Rating:F2}"));

            }

            context.Movies.AddRange(movies);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportHallSeats(CinemaContext context, string jsonString)
        {
            var hallsDtos = JsonConvert.DeserializeObject<ImportHallSeatDto[]>(jsonString);
            
            var sb = new StringBuilder();

            var halls = new List<Hall>();

            var seats = new List<Seat>();

            foreach (var hallDto in hallsDtos)
            {
                Hall hall = new Hall
                {
                    Name = hallDto.Name,
                    Is3D = hallDto.Is3D,
                    Is4Dx = hallDto.Is4Dx,
                };

                var isExist = halls.Any(m => m.Name == hallDto.Name);

                if (!IsValid(hall) || hallDto.Seats <= 0 || isExist)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                halls.Add(hall);

                for (int i = 0; i < hallDto.Seats; i++)
                {
                    var seat = new Seat
                    {
                        Hall = hall
                    };
                    seats.Add(seat);
                }

                string projectionType;

                if (hall.Is3D && hall.Is4Dx)
                {
                    projectionType = $"4Dx/3D";
                }
                else if (hall.Is3D && !hall.Is4Dx)
                {
                    projectionType = $"3D";
                }
                else if (!hall.Is3D && hall.Is4Dx)
                {
                    projectionType = $"4Dx";
                }
                else
                {
                    projectionType = $"Normal";
                }
                sb.AppendLine(String.Format(SuccessfulImportHallSeat, hall.Name, projectionType, hallDto.Seats));

            }

            context.Halls.AddRange(halls);
            context.Seats.AddRange(seats);
            
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProjections(CinemaContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(List<ImportProjectionDto>),
                new XmlRootAttribute("Projections"));
            
            var reader = new StringReader(xmlString);

            var projectionsDto = (List<ImportProjectionDto>)serializer.Deserialize(reader);
            
            var projections = new List<Projection>(); 

            var sb = new StringBuilder();

            foreach (var projectionDto in projectionsDto)
            {
                var movie = context.Movies.Find(projectionDto.MovieId);
                var hall = context.Halls.Find(projectionDto.HallId);

                var projection = Mapper.Map<Projection>(projectionDto);

                var isExist = projections.Any(m => m.HallId == projection.HallId 
                                                   && m.MovieId == projection.MovieId);

                if (!IsValid(projectionDto) || movie == null || hall == null || isExist)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                projections.Add(projection);
                
                sb.AppendLine(String.Format(SuccessfulImportProjection
                    ,movie.Title
                    ,projection.DateTime.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
            }

            context.Projections.AddRange(projections);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCustomerTickets(CinemaContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(List<ImportCustomerTicketsDto>),
                new XmlRootAttribute("Customers"));
            
            var reader = new StringReader(xmlString);

            var customersDto = (List<ImportCustomerTicketsDto>)serializer.Deserialize(reader);
            
            var customers = new List<Customer>();
            var tickets = new List<Ticket>();
            
            var sb = new StringBuilder();

            foreach (var customerDto in customersDto)
            {

                var customer = new Customer
                {
                    FirstName = customerDto.FirstName,
                    LastName = customerDto.LastName,
                    Age = int.Parse(customerDto.Age),
                    Balance = Decimal.Parse(customerDto.Balance),
                };
                var isValidTickets = customerDto.Tickets
                    .All(x => context.Projections.Any(p => p.Id == int.Parse(x.ProjectionId)));

                if (!IsValid(customer) || !isValidTickets)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                foreach (var ticketDto in customerDto.Tickets)
                {

                    if (context.Projections.Find(int.Parse(ticketDto.ProjectionId)) != null )//&& entity == 0)
                    {

                        var ticket = new Ticket
                        {
                            Customer = customer,
                            ProjectionId = int.Parse(ticketDto.ProjectionId),
                            Price = Decimal.Parse(ticketDto.Price)
                        };

                        tickets.Add(ticket);
                    }
                }

                customers.Add(customer);
                
                sb.AppendLine(String.Format(SuccessfulImportCustomerTicket,customer.FirstName,customer.LastName, customerDto.Tickets.Count));
            }

            context.Customers.AddRange(customers);
            context.Tickets.AddRange(tickets);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        private static bool IsValid(object entity)
        {
            var validationContext = new ValidationContext(entity);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(
                entity, validationContext, validationResult, true);

            return isValid;
        }
    }
}