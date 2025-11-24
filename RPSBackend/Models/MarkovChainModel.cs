namespace RpsBackend.Models;

public class NextMoveCounts
{
    public Dictionary<Move, int> Counts { get; } = new()
    {
        { Move.Rock, 0 },
        { Move.Paper, 0 },
        { Move.Scissors, 0 }
    };
}

public class MarkovChainData
{
    public Dictionary<(Move LastMove, Result LastResult), NextMoveCounts> Transitions { get; } = new();
    
    public MarkovChainData()
    {
        // Initialize all 9 states
        foreach (Move move in Enum.GetValues<Move>())
        {
            foreach (Result result in Enum.GetValues<Result>())
            {
                Transitions[(move, result)] = new NextMoveCounts();
            }
        }
    }
}
