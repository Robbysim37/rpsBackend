namespace RpsBackend.Services
{
    using RpsBackend.DTOs;


    /// Playground for testing and comparing different RPS algorithms.
    /// - Simulate algorithm vs algorithm (e.g., RNG vs first-order Markov)
    /// - Run human input sequences against a chosen algorithm
    /// - Collect stats and diagnostics for analysis
    /// 
    public class AlgorithmTestingService
    {
        private readonly RpsGameService _gameService;

        public AlgorithmTestingService(RpsGameService gameService)
        {
            _gameService = gameService;
        }

        public AlgorithmTestingResultsDto RNGvsRNG(int numberOfGames)
        {
            var results = new AlgorithmTestingResultsDto
            {
                numberOfGames = numberOfGames,
                wins = 0,
                losses = 0,
                ties = 0
            };

            for (int i = 0; i < numberOfGames; i++)
            {
                // Both players use RNG for this sim
                Move playerOneMove = _gameService.RandomMove();
                Move playerTwoMove = _gameService.RandomMove();

                // Pure logic — no DB write
                Result outcome = _gameService.playWithoutPersist(playerOneMove, playerTwoMove);

                switch (outcome)
                {
                    case Result.Win:
                        results.wins++;
                        break;
                    case Result.Loss:
                        results.losses++;
                        break;
                    case Result.Tie:
                        results.ties++;
                        break;
                }
            }

            // If you want rates instead of counts, you can calculate them on the caller/frontend.

            return results;
        }
    }
}