using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using RpsBackend.DTOs;
using RpsBackend.Services;

namespace RpsBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayController : ControllerBase
{
    private readonly RpsGameService _gameService;
    private readonly AlgorithmTestingService _algorithmTestingService;
    private readonly PredictionService _predictionService;

    public PlayController(
        RpsGameService gameService,
        AlgorithmTestingService algorithmTestingService,
        PredictionService predictionService)
    {
        _gameService = gameService;
        _algorithmTestingService = algorithmTestingService;
        _predictionService = predictionService;
    }

    // POST /api/play/run-simulation
    [HttpPost("run-simulation")]
    public ActionResult<AlgorithmTestingResultsDto> RunSimulation([FromBody] int numberOfGames)
    {
        return _algorithmTestingService.RNGvsRNG(numberOfGames);
    }

    // POST /api/play
    [HttpPost]
    public async Task<ActionResult<PlayResponseDto>> Play([FromBody] PlayRequestDto request)
    {
        var humanMoves = request.HumanMoves;

        var playerHistory = new List<MoveWithResult>();

        if (humanMoves.Length < playerHistory.Count)
        {
            return BadRequest("Move history must be at least as long as result history.");
        }

        for (int i = 0; i < request.PreviousHumanResults.Length; i++)
        {
            var move = request.HumanMoves[i];
            var gameResult = request.PreviousHumanResults[i];

            playerHistory.Add(new MoveWithResult(move, gameResult));
        }

        // validate enum (just in case)
        if (!_gameService.ValidMoves.Contains(humanMoves[humanMoves.Length - 1]))
        {
            return BadRequest("Invalid move.");
        }

        // FIX THIS!!!! game service should no longer take an array, just the current move.
        // aiMove should take in the history data

        var aiMove = _predictionService.PlayMove(playerHistory);

        var result = await _gameService.PlayAndPersistAsync(humanMoves, aiMove);

        var response = new PlayResponseDto
        {
            AiMove = aiMove,
            Winner = result
        };

        return Ok(response);
    }
}
