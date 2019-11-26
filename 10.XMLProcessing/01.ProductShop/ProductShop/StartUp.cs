using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using AutoMapper;
using ProductShop.Data;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            Mapper.Initialize(expression => expression
                .AddProfile(new ProductShopProfile()));

            using (var context = new ProductShopContext())
            {
                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();

                //var usersXml = File.ReadAllText("./../../../Datasets/users.xml");
                //var productsXml = File.ReadAllText("./../../../Datasets/products.xml");
                //var categoriesXml = File.ReadAllText("./../../../Datasets/categories.xml");
                //var categoriesProductsXml = File.ReadAllText("./../../../Datasets/categories-products.xml");

                //Import
                //Console.WriteLine(ImportUsers(context, usersXml));
                //Console.WriteLine(ImportProducts(context, productsXml));
                //Console.WriteLine(ImportCategories(context, categoriesXml));
                //Console.WriteLine(ImportCategoryProducts(context, categoriesProductsXml));

                //Export
                //Console.WriteLine(GetProductsInRange(context));
                //Console.WriteLine(GetSoldProducts(context));
                //Console.WriteLine(GetCategoriesByProductsCount(context));
                Console.WriteLine(GetUsersWithProducts(context));
            }
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(user => user.ProductsSold.Count > 0)
                .OrderByDescending(user => user.ProductsSold.Count())
                .Select(user => new UserAndProductsDto
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Age = user.Age,
                    SoldProducts = new SoldProductsDto
                    {
                        Count = user.ProductsSold.Count(),
                        Products = user.ProductsSold
                            .Select(product => new ProductsDto
                            {
                                Name = product.Name,
                                Price = product.Price
                            })
                            .OrderByDescending(dto => dto.Price)
                            .ToArray()
                    }
                })
                .Take(10)
                .ToArray();

            var result = new UsersAndCountDto
            {
                Count = context.Users.Count(user => user.ProductsSold.Any()),
                Users = users
            };

            var serializer = new XmlSerializer(typeof(UsersAndCountDto)
                , new XmlRootAttribute("Users"));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, result, namespaces);
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(category => new CategoryByProductsDto
                {
                    Name = category.Name,
                    Count = category.CategoryProducts.Count,
                    AveragePrice = category.CategoryProducts
                        .Average(product => product.Product.Price),
                    TotalRevenue = category.CategoryProducts
                        .Sum(product => product.Product.Price)
                })
                .OrderByDescending(dto => dto.Count)
                .ThenBy(dto => dto.TotalRevenue)
                .ToArray();
            //Get all categories.
            //For each category select
            //  its name, the
            //  number of products, the
            //  average price of those products and the
            //  total revenue (total price sum) of those products (regardless if they have a buyer or not).
            //Order them by the number of products (descending) then by total revenue.

            var serializer = new XmlSerializer(typeof(CategoryByProductsDto[]),
                new XmlRootAttribute("Categories"));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, categories, namespaces);
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(user => user.ProductsSold.Count > 0)
                .OrderBy(user => user.LastName)
                .ThenBy(user => user.FirstName)
                .Select(user => new UserSoldProductsDto
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Products = user.ProductsSold
                        .Select(product => new SoldProductDto
                        {
                            Name = product.Name,
                            Price = product.Price
                        }).ToArray()
                })
                .Take(5)
                .ToArray();

            var serializer = new XmlSerializer(typeof(UserSoldProductsDto[]),
                new XmlRootAttribute("Users"));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, users, namespaces);
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(product => product.Price >= 500 && product.Price <= 1000)
                .OrderBy(product => product.Price)
                .Select(product => new ProductInRangeDto
                {
                    Name = product.Name,
                    Price = product.Price,
                    Buyer = product.Buyer.FirstName + " " + product.Buyer.LastName
                })
                .Take(10)
                .ToArray();

            var serializer = new XmlSerializer(typeof(ProductInRangeDto[]),
                new XmlRootAttribute("Products"));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, products, namespaces);
            }

            return sb.ToString().TrimEnd();
        }
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportCategoryProductDto[]),
                new XmlRootAttribute("CategoryProducts"));

            var categoryProductsDto = (ImportCategoryProductDto[])serializer
                .Deserialize(new StringReader(inputXml));

            var categoryProducts = new List<CategoryProduct>();

            foreach (var categoryProductDto in categoryProductsDto)
            {
                var category = context.Categories.Find(categoryProductDto.CategoryId);
                var product = context.Products.Find(categoryProductDto.ProductId);

                if (category == null || product == null)
                {
                    continue;
                }
                var categoryProduct = Mapper.Map<CategoryProduct>(categoryProductDto);

                categoryProducts.Add(categoryProduct);
            }

            context.CategoryProducts.AddRange(categoryProducts.Distinct());

            var count = context.SaveChanges();

            return $"Successfully imported {count}";
        }
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportCategoryDto[]),
                new XmlRootAttribute("Categories"));

            var categoriesDto = (ImportCategoryDto[])serializer.Deserialize(new StringReader(inputXml));

            var categories = new List<Category>();

            foreach (var categoryDto in categoriesDto)
            {
                var category = Mapper.Map<Category>(categoryDto);

                categories.Add(category);
            }

            context.Categories.AddRange(categories);

            var count = context.SaveChanges();

            return $"Successfully imported {count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(ImportProductDto[]),
                new XmlRootAttribute("Products"));

            var products = new List<Product>();

            using (var reader = new StringReader(inputXml))
            {
                var productsDto = (ImportProductDto[])xmlSerializer.Deserialize(reader);

                foreach (var productDto in productsDto)
                {
                    var product = Mapper.Map<Product>(productDto);

                    products.Add(product);
                }
            }
            context.Products.AddRange(products);

            var count = context.SaveChanges();

            return $"Successfully imported {count}";
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportUserDto[]),
                new XmlRootAttribute("Users"));

            var users = new List<User>();

            using (var reader = new StringReader(inputXml))
            {
                var usersDto = (ImportUserDto[])serializer.Deserialize(reader);

                foreach (var userDto in usersDto)
                {
                    var user = Mapper.Map<User>(userDto);
                    users.Add(user);
                }
            }
            context.Users.AddRange(users);
            var count = context.SaveChanges();
            return $"Successfully imported {count}"; ;
        }
    }
}