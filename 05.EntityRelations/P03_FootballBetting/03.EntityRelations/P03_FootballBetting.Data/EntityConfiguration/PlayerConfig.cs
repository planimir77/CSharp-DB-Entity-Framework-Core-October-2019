using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P03_FootballBetting.Data.Models;

namespace P03_FootballBetting.Data.EntityConfiguration
{
    public class PlayerConfig : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            // PlayerId, Name, SquadNumber, TeamId, PositionId, IsInjured
            builder.HasKey(x => x.PlayerId);

            builder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(20);

            builder
                .Property(x => x.SquadNumber)
                .IsRequired();
        }
    }
}
