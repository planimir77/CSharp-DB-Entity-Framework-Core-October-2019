namespace SoftUni
{
    using Data;
    using System;
    using System.Linq;
    using System.Text;
    using Models;
    using System.Globalization;
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new SoftUniContext();

            var employees = DeleteProjectById(context);

            Console.WriteLine(employees);
        }

        public static string RemoveTown(SoftUniContext context)
        {
            var town = context.Towns.First(t => t.Name == "Seattle");

            var addresses = context.Addresses.Where(a => a.Town.Name == "Seattle").ToArray();

            var removedAddresses = addresses.Count();

            var employees = context.Employees.Where(e => e.Address.Town.Name == town.Name).ToArray();

            foreach (var employee in employees)
            {
                employee.AddressId = null;
            }

            context.Addresses.RemoveRange(addresses);

            context.Towns.Remove(town);

            context.SaveChanges();
            
            return $"{removedAddresses} addresses in Seattle were deleted";
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            var project = context.Projects.First(p => p.ProjectId == 2);
            var connectionToDelete = context.EmployeesProjects.Where(employeeProject => employeeProject.ProjectId == 2).ToArray();

            context.EmployeesProjects.RemoveRange(connectionToDelete);

            context.Projects.Remove(project);

            context.SaveChanges();

            return String.Join(Environment.NewLine, context.Projects.Select(project1 => project1.Name).Take(10));
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(employee => employee.FirstName.StartsWith("Sa"))
                .Select(employee => new
                {
                    employee.FirstName,
                    employee.LastName,
                    employee.JobTitle,
                    employee.Salary
                })
                .OrderBy(arg => arg.FirstName)
                .ThenBy(arg => arg.LastName);

            var result = new StringBuilder();

            foreach (var employee in employees)
            {
                result.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - (${employee.Salary:F2})");
            }
            return result.ToString().TrimEnd();
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(employee => employee.Department.Name == "Engineering" ||
                                   employee.Department.Name == "Tool Design" ||
                                   employee.Department.Name == "Marketing" ||
                                   employee.Department.Name == "Information Services")
                .OrderBy(employee => employee.FirstName)
                .ThenBy(employee => employee.LastName)
                .ToArray();

            var result = new StringBuilder();

            foreach (var employee in employees)
            {
                employee.Salary *= (decimal)1.12;

                result.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:F2})");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            var projects = context.Projects
                .OrderByDescending(project => project.StartDate)
                .Take(10)
                .OrderBy(arg => arg.Name)
                .ToArray();

            var result = new StringBuilder();

            foreach (var project in projects)
            {
                result.AppendLine($"{project.Name}");
                result.AppendLine($"{project.Description}");
                result.AppendLine($"{project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)}");
            }
            return result.ToString().TrimEnd();
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments = context.Departments
                .Where(department => department.Employees.Count > 5)
                .Select(department => new
                {
                    EmployeesCount = department.Employees.Count,
                    DepartmentName = department.Name,
                    MenagerFullName = department.Manager.FirstName + " " + department.Manager.LastName,
                    Employees = department.Employees
                        .Select(employee => new
                        {
                            employee.FirstName,
                            employee.LastName,
                            employee.JobTitle
                        })
                        .OrderBy(arg => arg.FirstName)
                        .ThenBy(arg => arg.LastName)
                        .ToArray()
                })
                .OrderBy(arg => arg.EmployeesCount)
                .ThenBy(arg => arg.DepartmentName)
                .ToArray();

            var result = new StringBuilder();

            foreach (var department in departments)
            {
                result.AppendLine($"{department.DepartmentName} - {department.MenagerFullName}");
                foreach (var employee in department.Employees)
                {
                    result.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                }
            }
            return result.ToString().TrimEnd();
        }

        public static string GetEmployee147(SoftUniContext context)
        {

            var result = new StringBuilder();

            var employee = context.Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    FullName = e.FirstName + " " + e.LastName,
                    e.JobTitle,
                    Projects = e.EmployeesProjects.Select(p => p.Project.Name).OrderBy(p => p).ToArray()
                })
                .First();
            result.AppendLine($"{employee.FullName} - {employee.JobTitle}");

            foreach (var project in employee.Projects)
            {
                result.AppendLine(project);
            }

            return result.ToString().TrimEnd();
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            var result = new StringBuilder();

            var addresses = context.Addresses
                .GroupBy(address => new
                {
                    address.AddressText,
                    TownName = address.Town.Name,
                    EmployeesCount = address.Employees.Count
                })
                .OrderByDescending(grouping => grouping.Key.EmployeesCount)
                .ThenBy(grouping => grouping.Key.TownName)
                .ThenBy(grouping => grouping.Key.AddressText)
                .Take(10)
                .ToArray();

            foreach (var address in addresses)
            {
                result.AppendLine($"{address.Key.AddressText}, {address.Key.TownName} - {address.Key.EmployeesCount} employees");
            }
            return result.ToString().TrimEnd();
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var result = new StringBuilder();
            var employeesInfo = context.Employees
                .Where(e => e.EmployeesProjects
                    .Any(p => (p.Project.StartDate.Year >= 2001
                              && p.Project.StartDate.Year <= 2003)))
                .Take(10)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    MenagerFirstName = e.Manager.FirstName,
                    MenagerLastName = e.Manager.LastName,
                    Projects = e.EmployeesProjects.Select(ep => new
                    {
                        ep.Project.Name,
                        ep.Project.StartDate,
                        ep.Project.EndDate
                    })
                })
                .ToArray();

            foreach (var employeeInfo in employeesInfo)
            {
                result.AppendLine($"{employeeInfo.FirstName} {employeeInfo.LastName} - Manager: {employeeInfo.MenagerFirstName} {employeeInfo.MenagerLastName}");


                foreach (var project in employeeInfo.Projects)
                {
                    result.Append($"--{project.Name} - {project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)} - ");

                    if (project.EndDate is null)
                    {
                        result.AppendLine("not finished");
                    }
                    else
                    {
                        result.AppendLine($"{project.EndDate?.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)}");
                    }
                }
            }

            return result.ToString().TrimEnd();
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var address = new Address
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            context.Add(address);

            context.Employees.First(e => e.LastName == "Nakov").Address = address;

            context.SaveChanges();

            var employeesAddresses = context.Employees.OrderByDescending(e => e.AddressId).Select(e => e.Address.AddressText).Take(10).ToArray();

            var result = new StringBuilder();

            foreach (var employeeAddress in employeesAddresses)
            {
                result.AppendLine(employeeAddress);
            }

            return result.ToString().TrimEnd();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.Department.Name == "Research and Development")
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .Select(e => new { e.FirstName, e.LastName, DepartmentName = e.Department.Name, e.Salary })
                .ToArray();

            var result = new StringBuilder();

            foreach (var e in employees)
            {
                result.AppendLine($"{e.FirstName} {e.LastName} from {e.DepartmentName} - ${e.Salary:F2}");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.Salary > 50000)
                .Select(x => new { x.FirstName, x.Salary })
                .OrderBy(e => e.FirstName)
                .ToArray();

            var result = new StringBuilder();

            foreach (var employee in employees)
            {
                result.AppendLine($"{employee.FirstName} - {employee.Salary:F2}");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var employees = context.Employees.ToArray();

            var result = new StringBuilder();

            foreach (var employee in employees.OrderBy(x => x.EmployeeId))
            {
                result.Append(
                    $"{employee.FirstName} {employee.LastName} ");
                if (employee.MiddleName != null)
                {
                    result.Append($"{employee.MiddleName} ");
                }
                result.AppendLine($"{employee.JobTitle} {employee.Salary:F2}");
            }

            return result.ToString().TrimEnd();
        }
    }
}
