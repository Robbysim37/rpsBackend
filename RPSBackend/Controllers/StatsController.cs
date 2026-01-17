using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RpsBackend.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

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
    public async Task<ActionResult<AllAnonymousGamesDto>> GetAllGames()
    {
        var response = new AllAnonymousGamesDto
        {
            anonymousGames = await _statsGatheringService.GetAllGames()
        };

        return Ok(response);
    }

    [Authorize]
    [HttpGet("user")]
    public async Task<IActionResult> GetUserStats()
    {
        // Validates that this request is tied to a valid user
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdStr, out var userId))
        {
            return Unauthorized();
        }

        //the type here is still anonymous games simply because I don't need to send back player ID or foreign keys
        var response = new AllAnonymousGamesDto
        {
            anonymousGames = await _statsGatheringService.GetAllUserGames(userId)
        };

        return Ok(response);
    }
}