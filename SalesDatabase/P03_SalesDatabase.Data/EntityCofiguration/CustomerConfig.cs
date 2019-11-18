using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P03_SalesDatabase.Data.Models;

namespace P03_SalesDatabase.Data.EntityCofiguration
{
    public class CustomerConfig : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(x => x.CustomerId);

            builder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode();

            builder
                .Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(80)
                .IsUnicode(false);
        }
    }
}
