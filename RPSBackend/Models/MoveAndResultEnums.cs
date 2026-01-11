public enum Move
{
    Rock = 0,
    Paper = 1,
    Scissors = 2
}

public enum Result
{
    Win  = 0,
    Loss = 1,
    Tie  = 2
}

public class MoveWithResult
{
    public Move Move { get; set; }
    public Result Result { get; set; }
    
    public MoveWithResult(Move move, Result result)
    {
        Move = move;
        Result = result;
    }
}