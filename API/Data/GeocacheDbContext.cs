using API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class GeocacheDbContext : DbContext
    {
        public GeocacheDbContext (DbContextOptions<GeocacheDbContext> options) : base(options)
        {
        }

        public DbSet<Geocache> Geocaches { get; set; }
        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .ToTable("Item")
                .HasIndex(i => i.Name)
                    .IsUnique();

            modelBuilder.Entity<Geocache>().ToTable("Geocache");
        }
    }
}
