using Microsoft.EntityFrameworkCore;
using SpaceXBackend.DataLayer.Models;

namespace SpaceXBackend.DataLayer.Data
{
    public class SpaceXDbContext : DbContext
    {
        public SpaceXDbContext(DbContextOptions<SpaceXDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
