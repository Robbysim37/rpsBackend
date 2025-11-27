using Microsoft.EntityFrameworkCore;
using RpsBackend.Data;
using RpsBackend.Models;

namespace RpsBackend.Services
{
    public class StatsGatheringService{

        private readonly AppDbContext _db;

        public StatsGatheringService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<AnonymousGame[]> GetAllAnonymousGames()
        {
            return await _db.AnonymousGames.AsNoTracking().ToArrayAsync();
        }
    }
}