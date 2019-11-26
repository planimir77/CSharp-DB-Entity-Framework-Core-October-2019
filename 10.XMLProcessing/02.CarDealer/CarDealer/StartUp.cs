﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using AutoMapper;
using AutoMapper.Configuration;
using CarDealer.Data;
using CarDealer.Dtos.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore.Internal;

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
            using (var context = new CarDealerContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var inputSuppliersXml = File.ReadAllText("./../../../Datasets/suppliers.xml");
                var inputPartsXml = File.ReadAllText("./../../../Datasets/parts.xml");
                var inputCarsXml = File.ReadAllText("./../../../Datasets/cars.xml");

                Console.WriteLine(ImportSuppliers(context, inputSuppliersXml));
                Console.WriteLine(ImportParts(context, inputPartsXml));
                Console.WriteLine(ImportCars(context, inputCarsXml));
            }

        }

        //  Import 'Cars' and unique entity in mapping table 'PartCar'
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<CarDto>),
                new XmlRootAttribute("Cars"));

            var reader = new StringReader(inputXml);

            var carsDto = (List<CarDto>)serializer.Deserialize(reader);

            var cars = new List<Car>();

            var partsCar = new List<PartCar>(); ;

            foreach (var carDto in carsDto)
            {
                var car = Mapper.Map<Car>(carDto);
                
                foreach (var idDto in carDto.Parts.PartsId)
                {
                    var entity = partsCar
                        .Count(partCar => partCar.Car == car &&
                                          partCar.PartId == idDto.Id);

                    if (context.Parts.Find(idDto.Id) != null && entity == 0)
                    {

                        var partCar = new PartCar
                        {
                            Car = car,
                            PartId = idDto.Id
                        };
                        partsCar.Add(partCar);
                    }

                }
                cars.Add(car);

            }
            context.Cars.AddRange(cars);

            context.PartCars.AddRange(partsCar);

            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }


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