using FootbalDataAPI.models;
using Microsoft.EntityFrameworkCore;
namespace Football.API.DATA
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Competition> Competitions { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<CompetitionTeam> CompetitionTeams { get; set; }

        public DbSet<TeamPlayer> TeamPlayers { get; set; }
        public DbSet<Player> Players { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Fluent API

            builder.Entity<CompetitionTeam>(compTeam =>
            {
                compTeam.HasKey(ct => new { ct.CompetitionId, ct.TeamId });

                compTeam.HasOne<Competition>(cp => cp.Competition)
                .WithMany(c => c.Teams)
                .HasForeignKey(cp => cp.CompetitionId);

                compTeam.HasOne<Team>(cp => cp.Team)
                .WithMany(c => c.Competitions)
                .HasForeignKey(cp => cp.TeamId);
            });

            builder.Entity<TeamPlayer>(teamPlayer =>
            {
                teamPlayer.HasKey(tp => new { tp.PlayerId, tp.TeamId });

                teamPlayer.HasOne<Team>(tm => tm.Team)
                .WithMany(t => t.Squad)
                .HasForeignKey(tm => tm.TeamId);

                teamPlayer.HasOne<Player>(tp => tp.Player)
                .WithMany(p => p.Teams)
                .HasForeignKey(tp => tp.PlayerId);
            });

            builder.Entity<Competition>(comp =>
            {
                comp.HasKey(c => c.Id);
            });

            builder.Entity<Team>(team =>
            {
                team.HasKey(t => t.Id);
            });

            builder.Entity<Player>(play =>
            {
                play.HasKey(p => p.Id);
            });

        }
    }
}