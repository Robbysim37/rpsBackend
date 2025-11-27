using Microsoft.AspNetCore.Mvc;
using RpsBackend.DTOs;
using RpsBackend.Services;

namespace RpsBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayController : ControllerBase
{
    private readonly RpsGameService _gameService;
    private readonly AlgorithmTestingService _algorithmTestingService;

    public PlayController(
        RpsGameService gameService,
        AlgorithmTestingService algorithmTestingService)
    {
        _gameService = gameService;
        _algorithmTestingService = algorithmTestingService;
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

        // validate enum (just in case)
        foreach(var humanMove in humanMoves){
            if (!_gameService.ValidMoves.Contains(humanMove))
        {
            return BadRequest("Invalid move.");
        }
        }

        var aiMove = _gameService.RandomMove();

        var result = await _gameService.PlayAndPersistAsync(humanMoves, aiMove);

        var response = new PlayResponseDto
        {
            AiMove = aiMove,
            Winner = result
        };

        return Ok(response);
    }
}
