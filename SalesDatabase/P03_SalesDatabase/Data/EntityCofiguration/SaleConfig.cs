using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P03_SalesDatabase.Data.Models;

namespace P03_SalesDatabase.Data.EntityCofiguration
{
    public class SaleConfig : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.HasKey(x => x.SaleId);

            builder.Property(e => e.Date)
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(e => e.Product)
                .WithMany(p => p.Sales)
                .HasForeignKey(e => e.ProductId);

            builder.HasOne(e => e.Customer)
                .WithMany(c => c.Sales)
                .HasForeignKey(e => e.CustomerId);

            builder.HasOne(e => e.Store)
                .WithMany(s => s.Sales)
                .HasForeignKey(e => e.StoreId);
        }
    }
}
