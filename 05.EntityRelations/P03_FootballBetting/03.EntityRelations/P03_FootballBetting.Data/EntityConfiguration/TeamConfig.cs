using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.SqlServer.Query.ExpressionTranslators.Internal;
using P03_FootballBetting.Data.Models;

namespace P03_FootballBetting.Data.EntityConfiguration
{
    public class TeamConfig : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            // TeamId, Name, LogoUrl, Initials (JUV, LIV, ARS…),
            // Budget, PrimaryKitColorId, SecondaryKitColorId, TownId
            var name = 

            builder.HasKey(x => x.TeamId);

            builder
                .Property(x => x.Name)
                .HasMaxLength(20)
                .IsRequired();

            builder
                .Property(x => x.LogoUrl)
                .IsUnicode(false);

            builder
                .Property(x => x.Initials)
                .HasDefaultValue(CountryInitials(builder));

            builder
                .HasMany(x => x.HomeGames)
                .WithOne(x => x.HomeTeam)
                .HasForeignKey(x => x.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(x => x.AwayGames)
                .WithOne(x => x.AwayTeam)
                .HasForeignKey(x => x.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(x => x.Players)
                .WithOne(x => x.Team)
                .HasForeignKey(x => x.TeamId);
        }

        private string CountryInitials(EntityTypeBuilder<Team> builder)
        {
            var name = builder.Property(x => x.Name).ToString();
            var initials = name.Substring(0, 3);
            return initials;
        }
    }
}
