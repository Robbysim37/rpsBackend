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

        public async Task<AnonymousGame[]> GetAllUserGames(int userId)
        {
            var userGames = await _db.UserGames
                .AsNoTracking()
                .Where(g => g.UserId == userId)
                .Select(g => new AnonymousGame
                {
                    HumanMove = g.HumanMove,
                    AiMove = g.AiMove,
                    HumansResult = g.HumansResult
                })
                .ToListAsync();

            return userGames.ToArray();
        }
        public async Task<AnonymousGame[]> GetAllGames()
        {
            var anonymous = await _db.AnonymousGames
                .AsNoTracking()
                .Select(g => new AnonymousGame
                {
                    HumanMove = g.HumanMove,
                    AiMove = g.AiMove,
                    HumansResult = g.HumansResult
                })
                .ToListAsync();

            var userGames = await _db.UserGames
                .AsNoTracking()
                .Select(g => new AnonymousGame
                {
                    HumanMove = g.HumanMove,
                    AiMove = g.AiMove,
                    HumansResult = g.HumansResult
                })
                .ToListAsync();

            return anonymous.Concat(userGames).ToArray();
        }
    }
}