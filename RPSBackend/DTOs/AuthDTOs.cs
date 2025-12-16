using RpsBackend.Models;

namespace RpsBackend.DTOs;

public class AuthUserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
}

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public AuthUserDto User { get; set; } = null!;
}

public class MeResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
}
