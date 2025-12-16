using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RpsBackend.DTOs;
using RpsBackend.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RpsBackend.Controllers;

[ApiController]
[Route("auth/[controller]")]
public class UserTokenController : ControllerBase
{
    private readonly IUser _userService;
    private readonly IJwt _jwtService;
    private readonly AuthService _authService;

    public UserTokenController(IUser userService, IJwt jwtService, AuthService authService)
    {
        _userService = userService;
        _jwtService = jwtService;
        _authService = authService;
    }

    [HttpPost]
    public async Task<IActionResult> ReceiveGoogleId([FromBody] GoogleIdTokenRequest request)
    {
        try
        {
            var payload = await _authService.VerifyGoogleIdAsync(request.IdToken);

            var user = await _userService.GetOrCreateFromGoogleAsync(payload);

            var appJwt = _jwtService.GenerateToken(user);

            return Ok(new AuthResponseDto
            {
                Token = appJwt,
                User = new AuthUserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    AvatarUrl = user.AvatarUrl
                }
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<MeResponseDto>> Me()
    {
        var userIdStr = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (!int.TryParse(userIdStr, out var userId))
            return Unauthorized();
            
        var user = await _userService.GetUserByIdAsync(userId);

        if (user is null)
            return Unauthorized(); 

        System.Diagnostics.Debug.WriteLine("hit");

        return Ok(new MeResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            AvatarUrl = user.AvatarUrl
        });
    }
}
