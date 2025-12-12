using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

[Authorize]
[ApiController]
[Route("api/me")]
public class MeController : ControllerBase
{
    [HttpGet]
    public IActionResult GetMe()
    {
        var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrWhiteSpace(userId))
            return Unauthorized();

        return Ok(new { userId = int.Parse(userId) });
    }
}
