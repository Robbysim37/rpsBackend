using Microsoft.AspNetCore.Mvc;
using RpsBackend.Models;
using RpsBackend.Services;

namespace RpsBackend.Controllers;

[ApiController]
[Route("api/[controller]")]

public class StatsController : ControllerBase
{

    private readonly StatsGatheringService _statsGatheringService;

    public StatsController(StatsGatheringService statsGatheringService)
    {
        _statsGatheringService = statsGatheringService;
    }

    // GET api/stats
    [HttpGet]
    public async Task<ActionResult<AnonymousGame[]>> GetAllGames()
    {
        var games = await _statsGatheringService.GetAllAnonymousGames();
        return Ok(games);
    }
}