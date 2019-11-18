using P01_StudentSystem.Data.Models.Enums;

namespace P01_StudentSystem.Data.Models
{
    public class Resource
    {
        public int ResourceId { get; set; }
        public string Name { get; set; }//(up to 50 characters, unicode)

        public string Url { get; set; }//(not unicode)

        public ResourceType ResourceType { get; set; }//(enum – can be Video, Presentation, Document or Other)

        public int CourseId { get; set; }
        public Course Course { get; set; }

    }
}
