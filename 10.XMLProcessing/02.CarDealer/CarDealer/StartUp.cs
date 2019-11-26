using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using AutoMapper;
using CarDealer.Data;
using CarDealer.Dtos.Export;
using CarDealer.Dtos.Import;
using CarDealer.Models;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {

            Mapper.Initialize(x =>
            {
                x.AddProfile<CarDealerProfile>();
            });

            var context = new CarDealerContext();
            
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            //var inputSuppliersXml = File.ReadAllText("./../../../Datasets/suppliers.xml");
            //var inputPartsXml = File.ReadAllText("./../../../Datasets/parts.xml");
            //var inputCarsXml = File.ReadAllText("./../../../Datasets/cars.xml");
            //var inputCustomerXml = File.ReadAllText("./../../../Datasets/customers.xml");
            //var inputSalesXml = File.ReadAllText("./../../../Datasets/sales.xml");

            //Import
            //Console.WriteLine(ImportSuppliers(context, inputSuppliersXml));
            //Console.WriteLine(ImportParts(context, inputPartsXml));
            //Console.WriteLine(ImportCars(context, inputCarsXml));
            //Console.WriteLine(ImportCustomers(context, inputCustomerXml));
            //Console.WriteLine(ImportSales(context, inputSalesXml));

            //Export
            //var result = GetCarsWithDistance(context);
            //var result = GetCarsFromMakeBmw(context);
            //var result = GetLocalSuppliers(context);
            //var result = GetCarsWithTheirListOfParts(context);
            //var result = GetTotalSalesByCustomer(context);
            var result = GetSalesWithAppliedDiscount(context);
            Console.WriteLine(result);
        }
        // Exports

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var items = context
                .Sales
                .Select(sale => new ExportSalesWithAppliedDiscountDto
                {
                    Car = new CarInfo
                    {
                        Make = sale.Car.Make,
                        Model = sale.Car.Model,
                        TravelledDistance = sale.Car.TravelledDistance
                    },
                    Discount = sale.Discount,
                    CustomerName = sale.Customer.Name,
                    Price = sale.Car.PartCars.Sum(car => car.Part.Price),
                    PriceWithDiscount = sale.Car.PartCars.Sum(car => car.Part.Price) * (100 - sale.Discount)/100
                })
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportSalesWithAppliedDiscountDto[]),
                new XmlRootAttribute("sales"));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, items, namespaces);
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers  = context
                .Customers
                .Where(customer => customer.Sales.Any())
                .Select(customer => new ExportTotalSalesByCustomerDto
                {
                    FullName = customer.Name,
                    BoughtCars = customer.Sales.Count,
                    SpentMoney = customer.Sales.SelectMany(s => s.Car.PartCars).Sum(cp => cp.Part.Price)
                               //customer.Sales.Sum(sale => sale.Car.PartCars.Sum(car => car.Part.Price))
                })
                .OrderByDescending(dto => dto.SpentMoney)
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportTotalSalesByCustomerDto[]),
                new XmlRootAttribute("customers"));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, customers, namespaces);
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var items = context
                .Cars
                .Select(car => new ExportCarsWithTheirListOfPartsDto
                {
                    Make = car.Make,
                    Model = car.Model,
                    TravelledDistance = car.TravelledDistance,
                    Parts = new ExportPartsDto
                    {
                        Parts = car.PartCars
                            .Select(partCar => new ExportPartDto
                            {
                                Name = partCar.Part.Name,
                                Price = Decimal.Round(partCar.Part.Price,2)
                            })
                            .OrderByDescending(dto => dto.Price)
                            .ToList()
                    }
                })
                .OrderByDescending(dto => dto.TravelledDistance)
                .ThenBy(dto => dto.Model)
                .Take(5)
                .ToList();
            //Get all cars along with their list of parts.
            //For the car get only make, model and travelled distance and
            //for the parts get only name and price and sort all parts by price (descending)
            //. Sort all cars by travelled distance (descending)
            //then by model (ascending).
            //Select top 5 records.
            var serializer = new XmlSerializer(typeof(List<ExportCarsWithTheirListOfPartsDto>),
                new XmlRootAttribute("cars"));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, items, namespaces);
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context
                .Suppliers
                .Where(supplier => supplier.IsImporter == false)
                .Select(supplier => new ExportLocalSuppliersDto
                {
                    Id = supplier.Id,
                    Name = supplier.Name,
                    PartsCount = supplier.Parts.Count
                })
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportLocalSuppliersDto[]),
                new XmlRootAttribute("suppliers"));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, suppliers, namespaces);
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var cars = context
                .Cars
                .Where(car => car.Make =="BMW")
                .OrderBy(car => car.Model)
                .ThenByDescending(car => car.TravelledDistance)
                .Select(car => new ExportCarFromMakeBmwDto
                {
                    Id = car.Id,
                    Model = car.Model,
                    Travelleddistance = car.TravelledDistance
                })
                .ToArray();
            var serializer = new XmlSerializer(typeof(ExportCarFromMakeBmwDto[]),
                new XmlRootAttribute("cars"));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, cars, namespaces);
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.TravelledDistance > 2000000)
                .Select(c => new ExportCarWithDistanceDto
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .ToArray();

            var xmlSerializer = new XmlSerializer(typeof(ExportCarWithDistanceDto[]), new XmlRootAttribute("cars"));

            var sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            xmlSerializer.Serialize(new StringWriter(sb), cars, namespaces);

            return sb.ToString().TrimEnd();
        }
        // Imports
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<ImportSaleDto>),
                new XmlRootAttribute("Sales"));

            var reader = new StringReader(inputXml);

            var salesDto = (List<ImportSaleDto>)serializer.Deserialize(reader);

            var sales = new List<Sale>();

            foreach (var saleDto in salesDto)
            {
                var sale = Mapper.Map<Sale>(saleDto);
                var car = context.Cars.FirstOrDefault(car1 => car1.Id == sale.CarId);
                if (car != null)
                {
                    sales.Add(sale);
                }

            }

            context.Sales.AddRange(sales);

            int salesCount = context.SaveChanges();

            return $"Successfully imported {salesCount}";
        }
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<ImportCustomerDto>),
                new XmlRootAttribute("Customers"));

            var reader = new StringReader(inputXml);

            var customersDto = (List<ImportCustomerDto>)serializer.Deserialize(reader);

            var customers = new List<Customer>();

            foreach (var customerDto in customersDto)
            {
                var customer = Mapper.Map<Customer>(customerDto);
                customers.Add(customer);
            }

            context.Customers.AddRange(customers);

            int customersCount = context.SaveChanges();

            return $"Successfully imported {customersCount}";
        }
        //public static string ImportCars(CarDealerContext context, string inputXml)
        //{
        //    var serializer = new XmlSerializer(typeof(List<CarDto>),
        //        new XmlRootAttribute("Cars"));

        //    var reader = new StringReader(inputXml);

        //    var carsDto = (List<CarDto>)serializer.Deserialize(reader);

        //    var cars = new List<Car>();

        //    var partsCar = new List<PartCar>();

        //    foreach (var carDto in carsDto)
        //    {
        //        var car = Mapper.Map<Car>(carDto);

        //        foreach (var idDto in carDto.Parts.PartsId)
        //        {
        //            var entity = partsCar
        //                .Count(partCar => partCar.Car == car &&
        //                                  partCar.PartId == idDto.Id);

        //            if (context.Parts.Find(idDto.Id) != null && entity == 0)
        //            {

        //                var partCar = new PartCar
        //                {
        //                    Car = car,
        //                    PartId = idDto.Id
        //                };
        //                partsCar.Add(partCar);
        //            }

        //        }
        //        cars.Add(car);

        //    }
        //    context.Cars.AddRange(cars);

        //    context.PartCars.AddRange(partsCar);

        //    context.SaveChanges();

        //    return $"Successfully imported {cars.Count}";
        //}
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<PartDto>),
                new XmlRootAttribute("Parts"));

            var reader = new StringReader(inputXml);

            var partsDto = (List<PartDto>)serializer.Deserialize(reader);

            var parts = new List<Part>();

            foreach (var partDto in partsDto)
            {
                var supplier = context.Suppliers.Find(partDto.SupplierId);
                if (supplier != null)
                {
                    var part = Mapper.Map<Part>(partDto);

                    parts.Add(part);
                }
            }

            context.Parts.AddRange(parts);

            int itemsCount = context.SaveChanges();

            return $"Successfully imported {itemsCount}";
        }
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<SupplierDto>),
                new XmlRootAttribute("Suppliers"));

            var reader = new StringReader(inputXml);

            var suppliersDto = (List<SupplierDto>)serializer.Deserialize(reader);

            var suppliers = new List<Supplier>();

            foreach (var supplierDto in suppliersDto)
            {
                var supplier = Mapper.Map<Supplier>(supplierDto);
                suppliers.Add(supplier);
            }

            context.Suppliers.AddRange(suppliers);

            int suppliersCount = context.SaveChanges();

            return $"Successfully imported {suppliersCount}";
        }
    }
}