using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data.EntityCofiguration;
using P03_SalesDatabase.Data.Models;

namespace P03_SalesDatabase.Data
{
    public class SalesContext : DbContext
    {
        private DbSet<Customer> Customers { get; set; }

        private DbSet<Product> Products { get; set; }

        private DbSet<Sale> Sales { get; set; }

        private DbSet<Store> Stores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CustomerConfig());

            modelBuilder.ApplyConfiguration(new ProductConfig());

            modelBuilder.ApplyConfiguration(new SaleConfig());

            modelBuilder.ApplyConfiguration(new StoreConfig());
        }
    }
}
