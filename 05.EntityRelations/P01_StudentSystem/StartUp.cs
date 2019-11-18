using System;
using System.Linq;
using P01_StudentSystem.Data;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var context = new StudentSystemContext())
            {
                var student = context.Students.First();
                Console.WriteLine(student.Name);
            }
        }
    }
}
