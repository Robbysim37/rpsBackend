namespace RpsBackend.Services;

public class PredictionService
{
    private readonly Move[] _playerHistory;
    private readonly RpsGameService _RpsGameService;
    private readonly int numberOfTrackedMoves;

    public PredictionService(Move[] playerHistory, RpsGameService rpsGameService)
    {
        _playerHistory = playerHistory;
        _RpsGameService = rpsGameService;
    }

    public Move[]? RecentMoveHistory()
    {
        if (_playerHistory.Length < numberOfTrackedMoves)
        {
            return null;
        }

        var recent = new Move[numberOfTrackedMoves];
        Array.Copy(
            _playerHistory,
            _playerHistory.Length - numberOfTrackedMoves,
            recent,
            0,
            numberOfTrackedMoves
        );

        return recent;
    }

    public Move PlayMove()
    {
        Move[]? recentMoveHistory = RecentMoveHistory();

        if (recentMoveHistory != null)
        {
            //LOGIC GOES HERE
            return _RpsGameService.RandomMove();
        }

        //No substantial user history, we default to randomness

        return _RpsGameService.RandomMove();
    }
}