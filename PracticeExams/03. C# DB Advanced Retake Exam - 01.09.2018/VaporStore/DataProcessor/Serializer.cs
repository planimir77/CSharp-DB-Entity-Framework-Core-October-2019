using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using VaporStore.Data.Models.Enum;
using VaporStore.DataProcessor.ExportDtos;
using Formatting = Newtonsoft.Json.Formatting;

namespace VaporStore.DataProcessor
{
    using System;
    using Data;

    public static class Serializer
    {
        public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
        {

            var gamesDto = context?.Genres
                .Where(genre => genreNames.Any(s => s == genre.Name))
            .Select(genre => new ExportAllGamesByGenresDto
            {
                Id = genre.Id,
                Genre = genre.Name,
                GamesDto = genre.Games
                    .Where(game => game.Purchases.Count > 0)
                    .Select(game => new ExportGameDto
                    {
                        Id = game.Id,
                        Title = game.Name,
                        Developer = game.Developer.Name,
                        Tags = String.Join(", ", game.GameTags.Select(tag => tag.Tag.Name)),
                        Players = game.Purchases.Count
                    })
                    .OrderByDescending(dto => dto.Players)
                    .ThenBy(dto => dto.Id)
                    .ToList(),
                TotalPlayers = genre.Games.Where(game => game.Purchases.Count > 0).Sum(game => game.Purchases.Count)
            })
            .OrderByDescending(genre => genre.TotalPlayers)
            .ThenBy(genre => genre.Id)
            .ToList();

            var result = JsonConvert.SerializeObject(gamesDto, Formatting.Indented);

            return result;
        }

        public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
        {
            //var storeTypeValue = Enum.Parse<PurchaseType>(storeType);
            var users = context.Users
                .Include(u => u.Cards)
                .ThenInclude(c => c.Purchases)
                .ThenInclude(p => p.Game)
                .ThenInclude(g => g.Genre)
                .ToArray()
                .Select(user => new ExportUserDto
                {
                    Username = user.Username,
                    Purchases = user.Cards.SelectMany(p => p.Purchases)
                        .Where(p => p.Type.ToString() == storeType)
                        .OrderBy(p => p.Date)
                        .Select(p => new PurchaseDto
                        {
                            Cvc = p.Card.Cvc,
                            Date = p.Date.ToString(@"yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                            Card = p.Card.Number,
                            Game = new GameDto
                            {
                                Title = p.Game.Name,
                                Genre = p.Game.Genre.Name,
                                Price = p.Game.Price
                            }
                        })
                        .ToArray(),
                    TotalSpent = user.Cards.Sum(c => c.Purchases
                        .Where(p => p.Type.ToString() == storeType)
                        .Sum(p => p.Game.Price))
                })
                .Where(p => p.Purchases.Any())
                .OrderByDescending(u => u.TotalSpent)
                .ThenBy(u => u.Username)
                .ToArray();
            //var users = context
            //    .Users
            //    .Select(user => new ExportUserDto()
            //    {
            //        Username = user.Username,
            //        Purchase = user.Cards
            //             .SelectMany(card => card.Purchases)
            //             .Where(purchase => purchase.Type.ToString() == storeType)
            //             .Select(purchase => new PurchaseDto
            //             {
            //                 Card = purchase.Card.Number,
            //                 Cvc = purchase.Card.Cvc,
            //                 Date = purchase.Date.ToString(@"yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
            //                 Game = new GameDto
            //                 {
            //                     Title = purchase.Game.Name,
            //                     Genre = purchase.Game.Genre.Name,
            //                     Price = purchase.Game.Price
            //                 }
            //             })
            //             .OrderBy(dto => dto.Date)
            //             .ToArray(),
            //        TotalSpent = user.Cards
            //            .SelectMany(card => card.Purchases)
            //             .Where(purchase => purchase.Type.ToString() == storeType)
            //             .Sum(purchase => purchase.Game.Price)
            //    })
            //    .Where(dto => dto.Purchase.Any())
            //    .OrderByDescending(dto => dto.TotalSpent)
            //    .ThenBy(dto => dto.Username)
            //    .ToArray();

            var serializer = new XmlSerializer(typeof(ExportUserDto[]),
                new XmlRootAttribute("Users"));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] {  XmlQualifiedName.Empty  });

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, users, namespaces);
            }

            return sb.ToString().TrimEnd();
        }
    }
}