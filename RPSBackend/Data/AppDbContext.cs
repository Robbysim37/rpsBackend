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
        public DbSet<AnonymousGame> AnonymousGames { get; set; } = null!;   
    }
}
