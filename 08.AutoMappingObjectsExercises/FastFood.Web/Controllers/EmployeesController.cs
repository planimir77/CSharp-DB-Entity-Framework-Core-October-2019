
namespace FastFood.Web.Controllers
{
    using System.Linq;
    using AutoMapper.QueryableExtensions;
    using Models;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Data;
    using ViewModels.Employees;

    public class EmployeesController : Controller
    {
        private readonly FastFoodContext context;
        private readonly IMapper mapper;

        public EmployeesController(FastFoodContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IActionResult Register()
        {
            var positions = this.context
                .Positions
                .ProjectTo<RegisterEmployeeViewModel>
                    (this.mapper.ConfigurationProvider)
                .ToList();

            return this.View(positions);
        }

        [HttpPost]
        public IActionResult Register(RegisterEmployeeInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Home");
            }

            var employee = this.mapper.Map<Employee>(model);

            Position employeePosition = this.context.Positions
                .FirstOrDefault(x => x.Name == model.PositionName);

            employee.PositionId = employeePosition.Id;

            this.context.Employees.Add(employee);

            this.context.SaveChanges();

            return 
                this.RedirectToAction
                    ("All", "Employees");
        }

        public IActionResult All()
        {
            var employees = this.context
                .Employees.ProjectTo<EmployeesAllViewModel>
                    (this.mapper.ConfigurationProvider)
                .ToList();

            return this.View(employees);
        }
    }
}
