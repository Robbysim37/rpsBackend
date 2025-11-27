using RpsBackend.Data;
using RpsBackend.Models;

namespace RpsBackend.Services
{
    public class RpsGameService
    {
        private readonly AppDbContext _db;

        // Valid moves are now enums
        private readonly Move[] _validMoves = { Move.Rock, Move.Paper, Move.Scissors };
        public IReadOnlyList<Move> ValidMoves => _validMoves;

        public RpsGameService(AppDbContext db)
        {
            _db = db;
        }

        public Result playWithoutPersist(Move humanMove, Move aiMove)
        {
            return ComputeResult(humanMove, aiMove);
        }

        
        /// Plays a game, persists it, and returns the result.
        /// 
        public async Task<Result> PlayAndPersistAsync(Move humanMove, Move aiMove)
        {
            var result = ComputeResult(humanMove, aiMove);

            var game = new AnonymousGame
            {
                HumanMove    = humanMove,
                AiMove       = aiMove,
                HumansResult = result
            };

            _db.AnonymousGames.Add(game);
            await _db.SaveChangesAsync();

            return result;
        }

        public Move RandomMove()
        {
            var rnd = Random.Shared;
            var index = rnd.Next(_validMoves.Length);
            return _validMoves[index];
        }

        private static Result ComputeResult(Move human, Move ai)
        {
            if (human == ai)
                return Result.Tie;

            return (human, ai) switch
            {
                (Move.Rock,     Move.Scissors) => Result.Win,
                (Move.Scissors, Move.Paper)    => Result.Win,
                (Move.Paper,    Move.Rock)     => Result.Win,
                _                               => Result.Loss
            };
        }
    }
}
