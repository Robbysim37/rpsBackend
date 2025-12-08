using Microsoft.AspNetCore.Mvc;
using RpsBackend.DTOs;

namespace RpsBackend.Controllers;

[ApiController]
[Route("auth/[controller]")]
public class UserTokenController : ControllerBase
{
    [HttpPost]
    public IActionResult ReceiveGoogleId([FromBody] GoogleIdTokenRequest request)
    {
        Console.WriteLine($"Received Google ID Token: {request.IdToken}");

        return Ok(new { message = "ID token received", tokenLength = request.IdToken?.Length });
    }
}