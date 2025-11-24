namespace RpsBackend.Services;

public class RpsAiService
{

    private readonly string[] _validMoves = { "R", "P", "S" };

    public IReadOnlyList<string> ValidMoves => _validMoves;

    public string GetWinner(string humanMove, string aiMove)
    {
        
        if (humanMove == aiMove)
            return "Tie";

        // Rock beats Scissors
        if (humanMove == "R" && aiMove == "S") return "Human";
        if (humanMove == "S" && aiMove == "R") return "AI";

        // Paper beats Rock
        if (humanMove == "P" && aiMove == "R") return "Human";
        if (humanMove == "R" && aiMove == "P") return "AI";

        // Scissors beats Paper
        if (humanMove == "S" && aiMove == "P") return "Human";
        if (humanMove == "P" && aiMove == "S") return "AI";

        // Should never get here
        return "Tie";
    
    }

    //big boy logic

    public string RandomMove()
    {
        var rnd = Random.Shared;
        var index = rnd.Next(_validMoves.Length);
        return ValidMoves[index];
    }
}