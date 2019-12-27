using Microsoft.EntityFrameworkCore;
using Solstice.API.models;
namespace Solstice.API.DATA
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Fluent API

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