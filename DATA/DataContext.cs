using FootbalDataAPI.models;
using Microsoft.EntityFrameworkCore;
using Solstice.API.models;
namespace Solstice.API.DATA
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }

        public DbSet<Competition> Competitions { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<CompetitionTeam> CompetitionTeams { get; set; }
        public DbSet<Player> Players { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Fluent API

            builder.Entity<CompetitionTeam>(compTeam => {
                compTeam.HasKey(ct => new { ct.CompetitionId, ct.TeamId});

                compTeam.HasOne<Competition>(cp => cp.Competition)
                .WithMany(c => c.Teams)
                .HasForeignKey(cp => cp.CompetitionId);

                compTeam.HasOne<Team>(cp => cp.Team)
                .WithMany(c => c.Competitions)
                .HasForeignKey(cp => cp.TeamId);
            });

            builder.Entity<Competition>(comp => {
                comp.HasKey(c => c.Id);
            });

            builder.Entity<Team>(team => {
                team.HasKey(t => t.Id);
            });

            builder.Entity<Player>(play => {
                play.HasKey(p => p.Id);

                play.HasOne(p => p.Team)
                .WithMany(t => t.Squad)
                .HasForeignKey(p => p.TeamId)
                .OnDelete(DeleteBehavior.Restrict);
            });
            


            builder.Entity<Contact>(contact =>
            {
                contact.HasKey(c => c.Id);

                contact.OwnsOne(
                    x => x.Name, name => 
                    {
                        name.Property(x => x.FirstName).HasColumnName("FirstName");
                        name.Property(x => x.LastName).HasColumnName("LastName");
                    }
                );

                contact.OwnsOne(
                   x => x.Address,
                   address =>
                   {
                       address.Property(x => x.City).HasColumnName("City");
                       address.Property(x => x.State).HasColumnName("State");
                       address.Property(x => x.Street).HasColumnName("Address");
                       address.Property(x => x.Number).HasColumnName("AddressNumber");
                       address.Property(x => x.ZipCode).HasColumnName("Zip");
                   });
            });

            builder.Entity<PhoneNumber>(phone => {
                phone.HasKey(p => p.Id);

                phone.HasOne(p => p.Contact)
                .WithMany(c => c.PhoneNumbers)
                .HasForeignKey(p => p.ContactId)
                .OnDelete(DeleteBehavior.Restrict);
            });

        }
    }
}