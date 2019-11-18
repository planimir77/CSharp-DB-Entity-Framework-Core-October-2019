using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P03_SalesDatabase.Data.Models;

namespace P03_SalesDatabase.Data.EntityCofiguration
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.ProductId);

            builder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode();

            builder.Property(x => x.Description)
                .HasMaxLength(250)
                .HasDefaultValue("No description");
        }
    }
}
