using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data.EntityConfiguration
{
    public class ResourceConfig : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            builder.HasKey(x => x.ResourceId);

            builder
                .Property(x => x.Name)
                .HasMaxLength(50)
                .IsUnicode()
                .IsRequired();

            builder
                .Property(x => x.Url)
                .IsRequired()
                .IsUnicode(false);

            builder
                .Property(x => x.ResourceType)
                .IsRequired();
        }
    }
}
