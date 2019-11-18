using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P03_FootballBetting.Data.Models;

namespace P03_FootballBetting.Data.EntityConfiguration
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.UserId);

            builder
                .Property(x => x.Username)
                .HasMaxLength(20)
                .IsRequired();

            builder
                .Property(x => x.Password)
                .HasMaxLength(20)
                .IsRequired();

            builder
                .Property(x => x.Email)
                .HasMaxLength(20)
                .IsRequired();

            builder
                .Property(x => x.Name)
                .HasMaxLength(20)
                .IsRequired();

            builder
                .HasMany(x => x.Bets)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);
        }
    }
}
