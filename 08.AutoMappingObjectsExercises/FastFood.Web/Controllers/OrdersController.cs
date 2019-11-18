
namespace FastFood.Web.Controllers
{
    using AutoMapper.QueryableExtensions;
    using Models;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;

    using Data;
    using ViewModels.Orders;

    public class OrdersController : Controller
    {
        private readonly FastFoodContext context;
        private readonly IMapper mapper;

        public OrdersController(FastFoodContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IActionResult Create()
        {
            var viewOrder = new CreateOrderViewModel
            {
                Items = this.context.Items
                    .Select(x => new
                    {
                        x.Id, x.Name
                    })
                    .ToDictionary
                        (
                        key => key.Id,
                        value => value.Name
                        ),

                Employees = this.context.Employees
                    .Select(x => new
                    {
                        x.Id, x.Name
                    })
                    .ToDictionary
                        (
                        key => key.Id, 
                        value => value.Name
                        )
            };

            return this.View(viewOrder);
        }

        [HttpPost]
        public IActionResult Create(CreateOrderInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Home");
            }

            var order = this.mapper.Map<Order>(model);

            order.DateTime = DateTime.Now;

            order.OrderItems.Add(new OrderItem
            {
                ItemId = model.ItemId,
                Order = order,
                Quantity = model.Quantity
            });

            this.context.Orders.Add(order);

            this.context.SaveChanges();

            return this.RedirectToAction("All", "Orders");
        }

        public IActionResult All()
        {
            var allOrders = this.context.Orders
                .ProjectTo<OrderAllViewModel>(this.mapper.ConfigurationProvider)
                .ToList();

            return this.View("All", allOrders);
        }
    }
}
