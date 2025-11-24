using Microsoft.AspNetCore.Mvc;
using RpsBackend.DTOs;
using RpsBackend.Services;

namespace RpsBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayController : ControllerBase
{
    private readonly RpsAiService _aiService;
    private readonly AlgorithmTestingService _algorithmTestingService;

    public PlayController(RpsAiService aiService, AlgorithmTestingService algorithmTestingService)
    {
        _aiService = aiService;
        _algorithmTestingService = algorithmTestingService;
    }

    // POST /api/play/run-simulation
    [HttpPost("run-simulation")]
    public ActionResult<AlgorithmTestingResultsDto> runSimulation([FromBody] int numberOfGames)
    {
        return _algorithmTestingService.RNGvsRNG(numberOfGames);
    }

    [HttpPost]
    public ActionResult<PlayResponseDto> Play([FromBody] PlayRequestDto request)
    {
        // 1) Validate human move
        if (string.IsNullOrWhiteSpace(request.HumanMove))
        {
            return BadRequest("HumanMove is required.");
        }

        var humanMove = request.HumanMove.Trim().ToUpperInvariant();

        if (!_aiService.ValidMoves.Contains(humanMove))
        {
            return BadRequest("HumanMove must be one of: 'R', 'P', 'S'.");
        }

        // 2) For now: pick a random AI move (we'll swap this for your predictor later)
        var aiMove = _aiService.RandomMove();

        // 3) Decide the winner
        var winner = _aiService.GetWinner(humanMove, aiMove);

        // 4) Build response DTO
        var response = new PlayResponseDto
        {
            AIMove = aiMove,
            Winner = winner // "Human", "Ai", or "Tie"
        };

        return Ok(response);
    }
}
