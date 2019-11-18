using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data.EntityConfiguration
{
    public class CourseConfig : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(x => x.CourseId);

            builder
                .Property(x => x.Name)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(80);

            builder
                .Property(x => x.Description)
                .IsRequired(false)
                .IsUnicode();
            builder
                .Property(x => x.StartDate)
                .IsRequired();
            builder
                .Property(x => x.EndDate)
                .IsRequired();
            builder
                .Property(x => x.Price)
                .IsRequired();

            builder
                .HasMany(x => x.HomeworkSubmissions)
                .WithOne(x => x.Course)
                .HasForeignKey(x => x.CourseId);

            builder
                .HasMany(x => x.Resources)
                .WithOne(x => x.Course)
                .HasForeignKey(x => x.CourseId);

            builder
                .HasMany(x => x.StudentsEnrolled)
                .WithOne(x => x.Course)
                .HasForeignKey(x => x.CourseId);
        }
    }
}
