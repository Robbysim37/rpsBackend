using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RpsBackend.Config;
using RpsBackend.Models;

namespace RpsBackend.Services
{
    public class JwtService : IJwt
    {
        private readonly JwtSettings _settings;

        public JwtService(IOptions<JwtSettings> options)
        {
            _settings = options.Value;
        }

        public string GenerateToken(User user)
        {
            var keyBytes = Encoding.UTF8.GetBytes(_settings.Key);
            var signingKey = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            // Keep claims minimal. The key one is your local user Id.
            var claims = new List<Claim>
            {
                // ✅ Standard ASP.NET “user id”
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),

                // ✅ Optional: keep JWT subject consistent too
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),

                new Claim(ClaimTypes.Name, user.Name ?? string.Empty),

                // ✅ Make this name explicit (avoid confusion with real Google sub)
                new Claim("google_id", user.GoogleId ?? string.Empty)
            };

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
