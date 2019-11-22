using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore.SqlServer.Query.ExpressionTranslators.Internal;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var context = new CarDealerContext())
            {
                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();
                //var inputJsonSuppliers = File.ReadAllText("./../../../Datasets/suppliers.json");
                //var inputJsonParts = File.ReadAllText("./../../../Datasets/parts.json");
                //var inputJsonCars = File.ReadAllText("./../../../Datasets/cars.json");
                //var inputJsonCustomers = File.ReadAllText("./../../../Datasets/customers.json");
                //var inputJsonSales = File.ReadAllText("./../../../Datasets/sales.json");

                //Console.WriteLine(ImportSuppliers(context, inputJsonSuppliers));
                //Console.WriteLine(ImportParts(context, inputJsonParts));
                //Console.WriteLine(ImportCars(context, inputJsonCars));
                //Console.WriteLine(ImportCustomers(context, inputJsonCustomers));
                //Console.WriteLine(ImportSales(context, inputJsonSales));

                //var expectedOutputValue = "[{\"fullName\":\"Emmitt Benally\",\"boughtCars\":2,\"spentMoney\":12176.73},{\"fullName\":\"Natalie Poli\",\"boughtCars\":3,\"spentMoney\":9376.39},{\"fullName\":\"Marcelle Griego\",\"boughtCars\":1,\"spentMoney\":6211.03},{\"fullName\":\"Zada Attwood\",\"boughtCars\":1,\"spentMoney\":3405.02},{\"fullName\":\"Lino Subia\",\"boughtCars\":1,\"spentMoney\":1246.67},{\"fullName\":\"Hai Everton\",\"boughtCars\":1,\"spentMoney\":267.32}]";
                //Console.WriteLine(expectedOutputValue.Length);
                //var partOfStr = expectedOutputValue.Substring(0,14);
                //Console.WriteLine(JsonConvert.SerializeObject(partOfStr, Formatting.Indented));
                Console.WriteLine(GetSalesWithAppliedDiscount(context));

            }
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Take(10)
                .Select(sale => new SalesDiscountsDto
                {
                    Car = new CarInfoDto
                    {
                        Make = sale.Car.Make,
                        Model = sale.Car.Model,
                        TravelledDistance = sale.Car.TravelledDistance
                    },
                    CustomerName = sale.Customer.Name,
                    Discount = $"{sale.Discount:F2}",
                    Price = $"{sale.Car.PartCars.Sum(car => car.Part.Price):F2}",
                    PriceWithDiscount = $"{sale.Car.PartCars.Sum(car => car.Part.Price) * (100 - sale.Discount)/100:F2}"
                })
                .ToList();

            var json = JsonConvert.SerializeObject(sales, Formatting.Indented);

            return json;
            //Get first 10 sales with information about
            //      the car,
            //      customer and
            //      price of the sale
            //          with and
            //          without discount. 
        }
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(customer => customer.Sales.Count > 0)
                .Select(customer => new CustomersTotalSalesDto
                {
                    FullName = customer.Name,
                    BoughtCars = customer.Sales.Count,
                    SpentMoney = customer.Sales
                        .Sum(sale => sale.Car.PartCars
                            .Sum(car => car.Part.Price))
                })
                .OrderByDescending(dto => dto.SpentMoney)
                .ThenByDescending(dto => dto.BoughtCars)
                .ToList();

            var result = JsonConvert.SerializeObject(customers, Formatting.Indented);

            return result;
        }
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context
                .Cars
                .Select(car => new CarWithPartsDto
                {
                    Car = new CarInfoDto
                    {
                        Make = car.Make,
                        Model = car.Model,
                        TravelledDistance = car.TravelledDistance
                    },
                    Parts = car.PartCars.Select(partCar => new PartInfoDto
                    {
                        Name = partCar.Part.Name,
                        Price = $"{partCar.Part.Price:F2}"
                    })
                         .ToList()
                })
                .ToList();

            var result = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return result;
        }
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(supplier => supplier.IsImporter == false)
                .Select(supplier => new LocalSuppliersDto
                {
                    Id = supplier.Id,
                    Name = supplier.Name,
                    PartsCount = supplier.Parts.Count
                });

            var result = JsonConvert.SerializeObject(suppliers, Formatting.Indented);

            return result;
        }
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {

            var json = context.Cars
                .Where(car => car.Make == "Toyota")
                .Select(car => new CarsFromMakeToyotaDto
                {
                    Id = car.Id,
                    Make = car.Make,
                    Model = car.Model,
                    TravelledDistance = car.TravelledDistance
                })
                .OrderBy(car => car.Model)
                .ThenByDescending(car => car.TravelledDistance)
                .ToList();

            var result = JsonConvert.SerializeObject(json, Formatting.Indented);

            return result;
        }
        public static string GetOrderedCustomers(CarDealerContext context)
        {

            var cars = context.Customers
                .OrderBy(customer => customer.BirthDate)
                .ThenBy(customer => customer.IsYoungDriver)
                .Select(customer => new CustomerDto
                {
                    Name = customer.Name,
                    BirthDate = customer.BirthDate.ToString("dd/MM/yyyy"),
                    IsYoungDriver = customer.IsYoungDriver
                })
                .ToList();

            var result = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return result;
        }
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var suppliersData = JsonConvert.DeserializeObject<Supplier[]>(inputJson);

            context.Suppliers.AddRange(suppliersData);

            context.SaveChanges();

            return $"Successfully imported {suppliersData.Length}.";
        }
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var parts = JsonConvert.DeserializeObject<List<Part>>(inputJson)
                .Where(part => context
                .Suppliers
                    .Select(x => x.Id)
                    .Contains(part.SupplierId))
                .ToList();

            context.Parts.AddRange(parts);

            context.SaveChanges();

            return $"Successfully imported {parts.Count}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var dto = JsonConvert.DeserializeObject<PartCarDto[]>(inputJson);

            var partsCar = new List<PartCar>();

            var cars = new List<Car>();

            foreach (var partCarDto in dto)
            {

                var car = new Car()
                {
                    Make = partCarDto.Make,
                    Model = partCarDto.Model,
                    TravelledDistance = partCarDto.TravelledDistance,
                };

                foreach (var partId in partCarDto.PartsId.Distinct())
                {
                    var partCar = new PartCar()
                    {
                        Car = car,
                        PartId = partId
                    };

                    partsCar.Add(partCar);
                }

                cars.Add(car);
            }

            context.Cars.AddRange(cars);
            context.PartCars.AddRange(partsCar);

            context.SaveChanges();

            return $"Successfully imported {cars.Count}.";
        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var suppliersData = JsonConvert.DeserializeObject<Customer[]>(inputJson);

            context.Customers.AddRange(suppliersData);

            context.SaveChanges();

            return $"Successfully imported {suppliersData.Length}.";
        }
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var suppliersData = JsonConvert.DeserializeObject<Sale[]>(inputJson);

            context.Sales.AddRange(suppliersData);

            context.SaveChanges();

            return $"Successfully imported {suppliersData.Length}.";
        }
    }
}