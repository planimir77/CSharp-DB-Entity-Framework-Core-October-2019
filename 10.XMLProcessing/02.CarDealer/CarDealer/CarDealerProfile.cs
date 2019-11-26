using System.Collections.Generic;
using AutoMapper;
using CarDealer.Models;
using CarDealer.Dtos.Import;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<SupplierDto, Supplier>();

            this.CreateMap<PartDto, Part>();

            this.CreateMap<CarDto, Car>();

            this.CreateMap<ImportCustomerDto, Customer>();

            this.CreateMap<ImportSaleDto, Sale>();
        }
    }
}
