﻿
namespace FastFood.Web.MappingConfiguration
{
    using ViewModels.Categories;
    using ViewModels.Employees;
    using ViewModels.Items;
    using ViewModels.Orders;
    using AutoMapper;
    using Models;
    using ViewModels.Positions;

    public class FastFoodProfile : Profile
    {
        public FastFoodProfile()
        {
            //Positions
            this.CreateMap<CreatePositionInputModel, Position>()
                .ForMember(x => x.Name,
                    y => y.MapFrom(s => s.PositionName.TrimEnd()));

            this.CreateMap<Position, PositionsAllViewModel>()
                .ForMember(x => x.Name,
                    y => y.MapFrom(s => s.Name));

            //  Employees
            this.CreateMap<Position, RegisterEmployeeViewModel>()
                .ForMember(x => x.PositionName,
                y => y.MapFrom(s => s.Name));

            this.CreateMap<RegisterEmployeeInputModel, Employee>()
                .ForMember(x => x.Position,
                    y => y.Ignore());

            this.CreateMap<Employee, EmployeesAllViewModel>()
                .ForMember(x => x.Position,
                    y => y.MapFrom(s => s.Position.Name));

            // Category
            this.CreateMap<CreateCategoryInputModel, Category>()
                .ForMember(x => x.Name,
                    y => y.MapFrom(s => s.CategoryName));

            // Items
            this.CreateMap<CreateItemInputModel, Item>();

            this.CreateMap<Category, CreateItemViewModel>()
                .ForMember(x => x.CategoryId,
                    y => y.MapFrom(s => s.Id))
                .ForMember(x => x.CategoryName,
                    y => y.MapFrom(s => s.Name));

            this.CreateMap<CategoryAllViewModel, Category>()
                .ForMember(x => x.Name,
                    y => y.MapFrom(s => s.Name));

            this.CreateMap<Item, ItemsAllViewModels>()
                .ForMember(x => x.Category,
                    y => y.MapFrom(s => s.Category.Name));
            // Orders
            this.CreateMap<CreateOrderInputModel, Order>()
                .ForMember(x=>x.EmployeeId,
                    y=>y.MapFrom(s=>s.EmployeeId))
                .ForMember(x=>x.Customer,
                    y=>y.MapFrom(s=>s.Customer));

            this.CreateMap<Order, OrderAllViewModel>()
                .ForMember(x => x.Employee,
                    y => y.MapFrom(s => s.Employee.Name))
                .ForMember(x => x.OrderId,
                y => y.MapFrom(s => s.Id))
                .ForMember(x => x.DateTime,
                    y => y.MapFrom(s => s.DateTime.ToString("g")));
        }
    }
}
