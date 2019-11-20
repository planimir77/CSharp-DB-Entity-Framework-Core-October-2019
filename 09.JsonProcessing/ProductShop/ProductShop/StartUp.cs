using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTO;
using ProductShop.Models;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var context = new ProductShopContext())
            {
                // var inputJson = File.ReadAllText("./../../../Datasets/categories-products.json");
                var result = GetCategoriesByProductsCount(context);

                Console.WriteLine(result);
            }
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<User[]>(inputJson);

            context.Users.AddRange(users);

            context.SaveChanges();

            return $"Successfully imported {users.Length}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<Product[]>(inputJson);

            context.Products.AddRange(products);

            context.SaveChanges();

            return $"Successfully imported {products.Length}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert.DeserializeObject<Category[]>(inputJson).Where(x => x.Name != null).ToArray();

            context.Categories.AddRange(categories);

            context.SaveChanges();

            return $"Successfully imported {categories.Length}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categoryProducts = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson);

            context.CategoryProducts.AddRange(categoryProducts);

            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Length}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(product => product.Price >= 500 && product.Price <= 1000)
                .OrderBy(product => product.Price)
                .Select(product => new
                {
                    name = product.Name,
                    price = product.Price,
                    seller = $"{product.Seller.FirstName} {product.Seller.LastName}"
                });

            var json = JsonConvert.SerializeObject(products, Formatting.Indented);

            return json;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(user => user.ProductsSold
                    .Any(product => product.Buyer != null))
                .OrderBy(user => user.LastName)
                .ThenBy(user => user.FirstName)
                .Select(user => new UserDTO
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    SoldProducts = user.ProductsSold
                        .Where(product => product.Buyer != null)
                        .Select(product => new SoldProductsDTO
                        {
                            Name = product.Name,
                            Price = product.Price,
                            BuyerFirstName = product.Buyer.FirstName,
                            BuyerLastName = product.Buyer.LastName
                        })
                        .ToList()
                })
                .ToList();

            var json = JsonConvert.SerializeObject(users, Formatting.Indented);

            return json;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .OrderByDescending(category => category.CategoryProducts.Count)
                .Select(category => new CategoriesByProductsCountDTO
                {
                    Category = category.Name,
                    ProductsCount = category.CategoryProducts.Count,
                    AveragePrice = category.CategoryProducts
                        .Average(product => product.Product.Price)
                        .ToString("F2"),
                    TotalRevenue  = category.CategoryProducts
                        .Sum(product => product.Product.Price)
                        .ToString("F2")
                })
                .ToList();

            var json = JsonConvert.SerializeObject(categories,Formatting.Indented);

            return json;
        }
    }
}