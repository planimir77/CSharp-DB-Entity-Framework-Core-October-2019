using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using TeisterMask.Data.Models;
using TeisterMask.Data.Models.Enums;
using TeisterMask.DataProcessor.ImportDto;

namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    using Data;

    public static class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(List<ImportProjectDto>),
                new XmlRootAttribute("Projects"));

            var reader = new StringReader(xmlString);

            var projectsDto = (List<ImportProjectDto>)serializer.Deserialize(reader);

            var projects = new List<Project>();

            var tasks = new List<Task>();

            var sb = new StringBuilder();

            foreach (var projectDto in projectsDto)
            {
                if (!IsValid(projectDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Project project = new Project
                {
                    Name = projectDto.Name,
                    OpenDate = DateTime.ParseExact
                        (projectDto.OpenDate, @"dd/MM/yyyy", CultureInfo.InvariantCulture),
                    DueDate = string.IsNullOrEmpty(projectDto.DueDate) 
                        ? (DateTime?) null
                        : DateTime.ParseExact
                            (projectDto.DueDate, @"dd/MM/yyyy", CultureInfo.InvariantCulture)
                };

                var projectTasks = new List<Task>();

                foreach (var taskDto in projectDto.TasksDto)
                {
                    if (!IsValid(taskDto) )
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isValidEnumLabelType = Enum.IsDefined(typeof(LabelType), int.Parse(taskDto.LabelType, CultureInfo.InvariantCulture));
                    bool isValidEnumExecutionType = Enum.IsDefined(typeof(ExecutionType), int.Parse(taskDto.ExecutionType, CultureInfo.InvariantCulture));

                    var openDate = DateTime.ParseExact(taskDto.OpenDate, @"dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var dueDate = DateTime.ParseExact(taskDto.DueDate, @"dd/MM/yyyy", CultureInfo.InvariantCulture);
                    if (openDate < project.OpenDate || dueDate > project.DueDate  || !isValidEnumExecutionType || !isValidEnumLabelType)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var labelType = Enum.Parse<LabelType>(taskDto.LabelType);
                    var executionType = Enum.Parse<ExecutionType>(taskDto.ExecutionType);
                    Task task = new Task
                    {
                        Name = taskDto.Name,
                        OpenDate = openDate,
                        DueDate = dueDate,
                        ExecutionType = executionType,
                        LabelType = labelType,
                        Project = project
                    };

                    projectTasks.Add(task);
                }
                projects.Add(project);

                tasks.AddRange(projectTasks);

                sb.AppendLine(String.Format(CultureInfo.InvariantCulture,SuccessfullyImportedProject,
                    project.Name, projectTasks.Count));
            }

            context.Projects.AddRange(projects);

            context.Tasks.AddRange(tasks);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            var employeesDto = JsonConvert.DeserializeObject<ImportEmployeeDto[]>(jsonString);
            var employees = new List<Employee>();
            var employeeTasks= new List<EmployeeTask>();
            var sb = new StringBuilder();
            
            foreach (var employeeDto in employeesDto)
            {
                
                if (!IsValid(employeeDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Employee employee = new Employee
                {
                    Username = employeeDto.Username,
                    Email = employeeDto.Email,
                    Phone = employeeDto.Phone
                };

                employees.Add(employee);

                var currentEmployeeTasks = new List<EmployeeTask>();
                
                foreach (int dtoTaskId in employeeDto.TasksId.ToHashSet())
                {

                    Task isTaskExist = context.Tasks.Find(dtoTaskId);

                    if (isTaskExist == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var employeeTask = new EmployeeTask
                    {
                        Employee = employee,
                        TaskId = dtoTaskId
                    };

                    currentEmployeeTasks.Add(employeeTask);
                }
                employeeTasks.AddRange(currentEmployeeTasks);
                sb.AppendLine(String.Format(CultureInfo.InvariantCulture, SuccessfullyImportedEmployee, employee.Username, currentEmployeeTasks.Count));
            }

            context.Employees.AddRange(employees);
            context.EmployeesTasks.AddRange(employeeTasks);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}