using Microsoft.EntityFrameworkCore;
using RpsBackend.Models;

namespace RpsBackend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSets = tables

        public DbSet<User> Users { get; set; } = default!;

        public DbSet<AnonymousGame> AnonymousGames { get; set; } = null!;   
        
        public DbSet<UserGame> UserGames { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.GoogleId)
                .IsUnique();
        }
    }
}
