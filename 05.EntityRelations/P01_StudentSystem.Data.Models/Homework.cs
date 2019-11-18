using System;
using P01_StudentSystem.Data.Models.Enums;

namespace P01_StudentSystem.Data.Models
{
    public class Homework
    {
        public int HomeworkId { get; set; }

        public string Content { get; set; }//(string, linking to a file, not unicode)

        public ContentType ContentType { get; set; }//(enum – can be Application, Pdf or Zip)

        public DateTime SubmissionTime { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

    }
}
