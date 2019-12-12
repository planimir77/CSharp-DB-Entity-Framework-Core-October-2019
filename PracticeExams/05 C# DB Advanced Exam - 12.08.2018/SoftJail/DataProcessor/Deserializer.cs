using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;
using SoftJail.Data.Models;
using SoftJail.DataProcessor.ImportDto;

namespace SoftJail.DataProcessor
{

    using Data;
    using System;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            var itemsDtos = JsonConvert.DeserializeObject<ImportDepartmentDto[]>(jsonString);
            // Don't use new HashSet<T>(); in Dto Class use List<T> or T[]
            var sb = new StringBuilder();

            var departments = new List<Department>();

            var cells = new List<Cell>();

            foreach (var itemDto in itemsDtos)
            {
                Department department =new Department();

                //if (!IsValid(item) || !item.Albums.All(IsValid))
                //{
                //    sb.AppendLine(ErrorMessage);
                //    continue;
                //}

                //items.Add(item);

                //if (item.PhoneNumber == null)
                //{
                //    sb.AppendLine(String.Format(SuccessfullyImportedItemWithNoPhone,
                //        item.Name, item.Albums.Count));
                //}
                //else
                //{
                //    sb.AppendLine(String.Format(SuccessfullyImportedItemWithPhone,
                //        item.Name, item.PhoneNumber, item.Albums.Count));
                //}

            }

            //context.Items.AddRange(items);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            throw new NotImplementedException();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            throw new NotImplementedException();
        }
        public static bool IsValid(object entity)
        {
            var validationContext = new ValidationContext(entity);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(
                entity, validationContext,validationResult, true);

            return isValid;
        }
    }
}