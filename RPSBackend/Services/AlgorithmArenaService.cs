namespace RpsBackend.Services;
using RpsBackend.DTOs;



/// Playground for testing and comparing different RPS algorithms.
/// - Simulate algorithm vs algorithm (e.g., RNG vs first-order Markov)
/// - Run human input sequences against a chosen algorithm
/// - Collect stats and diagnostics for analysis

public class AlgorithmTestingService
{

    private readonly RpsAiService _aiService;

    public AlgorithmTestingService(RpsAiService aiService)
    {
        _aiService = aiService;
    }

    public AlgorithmTestingResultsDto RNGvsRNG(int numberOfGames)
    {
        var collectionOfGames = new AlgorithmTestingResultsDto
        {
            numberOfGames = numberOfGames
        };

        for (int i = 0; i < numberOfGames; i++)
        {
            string playerOneMove = _aiService.RandomMove();
            string playerTwoMove = _aiService.RandomMove();

            if (_aiService.GetWinner(playerOneMove,playerTwoMove) == "Human")
            {
                collectionOfGames.wins += 1;
            }
            else if (_aiService.GetWinner(playerOneMove,playerTwoMove) == "AI")
            {
                collectionOfGames.losses += 1;
            } 
            else
            {
                collectionOfGames.ties += 1;
            }
        }

        

        collectionOfGames.wins = collectionOfGames.wins / numberOfGames;
        collectionOfGames.losses = collectionOfGames.losses / numberOfGames;
        collectionOfGames.ties = collectionOfGames.ties / numberOfGames;

        return collectionOfGames;
    }
}
