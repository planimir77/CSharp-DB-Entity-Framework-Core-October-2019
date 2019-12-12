using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor
{
    using Data;
    using System;
    using ExportDto;
    using System.Linq;
    using Newtonsoft.Json;
    using System.Globalization;
    using Formatting = Newtonsoft.Json.Formatting;

    public static class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            var items = context.Projects
                .Where(project => project.Tasks.Any())
                .OrderByDescending(project => project.Tasks.Count)
                .ThenBy(project => project.Name)
                .Select(project => new ProjectExportDto
                {
                    ProjectName = project.Name,
                    HasEndDate = project.DueDate == null ? "No" : "Yes",
                    Tasks = project.Tasks
                        .Select(task => new TaskExportDto
                        {
                            TaskName = task.Name,
                            LabelType = task.LabelType.ToString()
                        })
                        .OrderBy(dto => dto.TaskName)
                        .ToArray(),
                    TasksCount = project.Tasks.Count.ToString()
                })
                .ToArray();
            
            var serializer = new XmlSerializer(typeof(ProjectExportDto[]),
                new XmlRootAttribute("Projects"));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, items, namespaces);
            }

            return sb.ToString().TrimEnd();
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var employees = context.Employees
                .Where(employee => employee.EmployeesTasks
                    .Any(task => task.Task.OpenDate >= date))
                .OrderByDescending(employee => employee.EmployeesTasks
                    .Count(task => task.Task.OpenDate >= date))
                .ThenBy(employee => employee.Username)
                .Select(employee => new ExportMostBusiestEmployeesDto
                {
                    Username = employee.Username,
                    Tasks = employee.EmployeesTasks
                        .Where(task => task.Task.OpenDate >= date)
                        .Select(task => new TaskExportDto
                        {
                            TaskName = task.Task.Name,
                            OpenDate = task.Task.OpenDate
                            .ToString(@"MM/dd/yyyy", CultureInfo.InvariantCulture),
                            DueDate = task.Task.DueDate
                            .ToString(@"MM/dd/yyyy", CultureInfo.InvariantCulture),
                            LabelType = task.Task.LabelType.ToString(),
                            ExecutionType = task.Task.ExecutionType.ToString()
                        })
                        .OrderByDescending(dto => DateTime
                            .ParseExact(dto.DueDate, @"MM/dd/yyyy", CultureInfo.InvariantCulture))
                        .ThenBy(dto => dto.TaskName)
                        .ToArray()
                })
                .Take(10)
                .ToList();


            var result = JsonConvert.SerializeObject(employees, Formatting.Indented);

            return result;
        }
    }
}