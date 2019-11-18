using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data.EntityConfiguration
{
    public class HomeworkConfig : IEntityTypeConfiguration<Homework>
    {
        public void Configure(EntityTypeBuilder<Homework> builder)
        {
            builder.HasKey(x => x.HomeworkId);

            builder
                .Property(x => x.Content)
                .IsRequired()
                .IsUnicode(false);

            builder
                .Property(x => x.ContentType)
                .IsRequired();

            builder
                .Property(x => x.SubmissionTime)
                .IsRequired();
        }
    }
}
