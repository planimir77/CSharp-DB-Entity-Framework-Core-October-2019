using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using AutoMapper;
using Newtonsoft.Json;
using VaporStore.Data.Models;
using VaporStore.Data.Models.Enum;
using VaporStore.DataProcessor.ImportDtos;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace VaporStore.DataProcessor
{
    using System;
    using Data;

    public static class Deserializer
    {
        private const string ErrorMessage = "Invalid Data";
        private const string SuccessfullyImportedGamesDevelopersGenresAndTags = "Added {0} ({1}) with {2} tags";
        private const string SuccessfullyImportedUserWithCards = "Imported {0} with {1} cards";
        private const string SuccessfullyImportedPurchase = "Imported {0} for {1}";
        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            var gamesDtos = JsonConvert.DeserializeObject<ImportGameDto[]>(jsonString);

            var sb = new StringBuilder();

            var games = new List<Game>();
            var developers = new List<Developer>();
            var genres = new List<Genre>();
            var tags = new List<Tag>();
            var gameTags = new List<GameTag>();


            foreach (var gameDto in gamesDtos)
            {
                if (!IsValid(gameDto) || gameDto.TagsDto.Count < 1 ||
                    gameDto.TagsDto.Any(string.IsNullOrEmpty))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Developer dev = developers.FirstOrDefault(d => d.Name == gameDto.Developer) ?? new Developer
                {
                    Name = gameDto.Developer
                };
                developers.Add(dev);

                Genre genre = genres.FirstOrDefault(g => g.Name == gameDto.Genre) ?? new Genre
                {
                    Name = gameDto.Genre
                };
                genres.Add(genre);

                Game game = new Game
                {
                    Name = gameDto.Name,
                    Developer = dev,
                    Genre = genre,
                    Price = gameDto.Price,
                    ReleaseDate = DateTime.ParseExact(gameDto.ReleaseDate,
                        @"yyyy-MM-dd", CultureInfo.InvariantCulture)
                };

                if (!IsValid(game))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var currentTag = new List<Tag>();

                foreach (var dtoTag in gameDto.TagsDto)
                {
                    if (currentTag.Any(tag1 => tag1.Name == dtoTag))
                    {
                        continue;
                    }
                    Tag tag = tags.FirstOrDefault(t => t.Name == dtoTag) ?? new Tag { Name = dtoTag };

                    tags.Add(tag);
                    currentTag.Add(tag);

                    var gameTag = new GameTag
                    {
                        Game = game,
                        Tag = tag
                    };
                    gameTags.Add(gameTag);
                }

                if (!IsValid(game) || !IsValid(dev) || !IsValid(genre))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                games.Add(game);

                sb.AppendLine(String.Format(CultureInfo.InvariantCulture,SuccessfullyImportedGamesDevelopersGenresAndTags,
                    game.Name, game.Genre.Name, currentTag.Count));

            }
            context?.Developers.AddRange(developers);
            context?.Genres.AddRange(genres);
            context?.Games.AddRange(games);
            context?.Tags.AddRange(tags);
            context?.GameTags.AddRange(gameTags);

            context?.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            var usersDto = JsonConvert.DeserializeObject<ImportUsersDto[]>(jsonString);

            var users = new List<User>();
            var cards = new List<Card>();

            var sb = new StringBuilder();
            foreach (var userDto in usersDto)
            {
                bool isValidTypes = userDto.CardsDto.Any(dto => Enum.IsDefined(typeof(CardType), dto.Type));

                if (!IsValid(userDto) || userDto.CardsDto.Count < 1 || !userDto.CardsDto.All(IsValid) || !isValidTypes)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                User user = new User()
                {
                    Username = userDto.Username,
                    FullName = userDto.FullName,
                    Email = userDto.Email,
                    Age = userDto.Age
                };

                foreach (var cardDto in userDto.CardsDto)
                {
                    var card = new Card
                    {
                        Number = cardDto.Number,
                        Cvc = cardDto.Cvc,
                        Type = Enum.Parse<CardType>(cardDto.Type),
                        User = user
                    };

                    users.Add(user);
                    cards.Add(card);
                }

                sb.AppendLine(String.Format(CultureInfo.InvariantCulture, SuccessfullyImportedUserWithCards, user.Username, userDto.CardsDto.Count));

            }
            context?.Users.AddRange(users);
            context?.Cards.AddRange(cards);
            context?.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(List<ImportPurchasesDto>),
                new XmlRootAttribute("Purchases"));

            var reader = new StringReader(xmlString);

            var purchasesDto = (List<ImportPurchasesDto>)serializer.Deserialize(reader);

            var purchases = new List<Purchase>();

            var sb = new StringBuilder();

            foreach (var purchaseDto in purchasesDto)
            {
                bool isTypeValid = Enum.IsDefined(typeof(PurchaseType), purchaseDto.Type);
                var card = context.Cards.FirstOrDefault(card1 => card1.Number == purchaseDto.Card);
                var game = context.Games.FirstOrDefault(game1 => game1.Name == purchaseDto.Title);

                if (!IsValid(purchaseDto) || !isTypeValid || card == null || game == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var purchase = new Purchase
                {
                    Type = Enum.Parse<PurchaseType>(purchaseDto.Type),
                    Card = card,
                    Game = game,
                    ProductKey = purchaseDto.Key,
                    Date = DateTime.ParseExact(purchaseDto.Date, @"dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)
                };

                if (!IsValid(purchase))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                purchases.Add(purchase);
                // TO DO FORMAT
                sb.AppendLine(String.Format(CultureInfo.InvariantCulture, SuccessfullyImportedPurchase, purchase.Game.Name, purchase.Card.User.Username));
            }

            context.Purchases.AddRange(purchases);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        private static bool IsValid(object entity)
        {
            ValidationContext validationContext = new ValidationContext(entity);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(
                entity, validationContext, validationResult, true);
        }
    }
}