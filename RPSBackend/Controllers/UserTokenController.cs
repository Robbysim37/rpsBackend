using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using RpsBackend.DTOs;
using RpsBackend.Services;

namespace RpsBackend.Controllers;

[ApiController]
[Route("auth/[controller]")]
public class UserTokenController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IUser _userService;
    private readonly IJwt _jwtService;

// inject it in constructor
public UserTokenController(IConfiguration config, IUser userService, IJwt jwtService)
{
    _config = config;
    _userService = userService;
    _jwtService = jwtService;
}

    [HttpPost]
    public async Task<IActionResult> ReceiveGoogleId([FromBody] GoogleIdTokenRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.IdToken))
        {
            return BadRequest(new { error = "Missing idToken" });
        }

        var googleClientId = _config["GoogleAuth:ClientId"];
        if (string.IsNullOrWhiteSpace(googleClientId))
        {
            return StatusCode(500, new { error = "Google client ID is not configured" });
        }

        GoogleJsonWebSignature.Payload payload;
        try
        {
            payload = await GoogleJsonWebSignature.ValidateAsync(
                request.IdToken,
                new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { googleClientId }
                });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Invalid Google ID token: {ex.Message}");
            return Unauthorized(new { error = "Invalid Google ID token" });
        }

        // At this point, token is valid and we have a trusted payload

        var user = await _userService.GetOrCreateFromGoogleAsync(payload);

        var appJwt = _jwtService.GenerateToken(user);

        return Ok(new
        {
            token = appJwt,
            user = new { user.Id, user.Name, user.AvatarUrl }
        });
    }
}
