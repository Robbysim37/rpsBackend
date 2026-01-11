using Microsoft.AspNetCore.Http.Features;
using RpsBackend.Models;

namespace RpsBackend.Services;

public class PredictionService
{
    private readonly RpsGameService _RpsGameService;

    public PredictionService( RpsGameService rpsGameService)
    {
        _RpsGameService = rpsGameService;
    }

    public List<MoveWithResult>? RecentMoveHistory(List<MoveWithResult> playerHistory)
    {
        const int numberOfTrackedMoves = 7;

        if (playerHistory.Count < numberOfTrackedMoves)
        {
            return null;
        }

        return playerHistory.GetRange(
            playerHistory.Count - numberOfTrackedMoves,
            numberOfTrackedMoves
        );
    }


    public MarkovChainData GetRawCounts(List<MoveWithResult> recentHistory)
    {
        MarkovChainData markovData = new MarkovChainData();

        for(int i = 0; i < recentHistory.Count -1; i++)
        {
            var currentItem = recentHistory[i];
            var nextItem = recentHistory[i + 1];

            markovData.Transitions[(currentItem.Move, currentItem.Result)].Counts[nextItem.Move]++;
        }

        return markovData;
    }

    public Move CalculateEVs(MoveWithResult currentState, MarkovChainData markovCounts)
    {
        var stateCounts = markovCounts.Transitions[(currentState.Move, currentState.Result)];

        int rockCounts = stateCounts.Counts[Move.Rock];
        int paperCounts = stateCounts.Counts[Move.Paper];
        int scissorCounts = stateCounts.Counts[Move.Scissors];
        int totatCounts = rockCounts + paperCounts + scissorCounts;

        if(totatCounts == 0)
        {
            return _RpsGameService.RandomMove();
        }
        
        int EVRock = scissorCounts - paperCounts;
        int EVPaper = rockCounts - scissorCounts;
        int EVScissors = paperCounts - rockCounts;

        Move bestMove = Move.Rock;
        int bestEV = EVRock;

        if (EVPaper > bestEV)
        {
            bestEV = EVPaper;
            bestMove = Move.Paper;
        }
        if (EVScissors > bestEV)
        {
            bestEV = EVScissors;
            bestMove = Move.Scissors;
        }
            
        return bestMove;

    }

    public Move PlayMove(List<MoveWithResult> playerHistory)
    {
        List<MoveWithResult>? recentHistory = RecentMoveHistory(playerHistory);
        MarkovChainData markovCounts;
        Move moveToPlay;

        if (recentHistory != null)
        {
            markovCounts = GetRawCounts(recentHistory);
            moveToPlay = CalculateEVs(recentHistory[recentHistory.Count -1], markovCounts);
            return moveToPlay;
        }

        //No substantial user history, we default to randomness

        return _RpsGameService.RandomMove();
    }
}