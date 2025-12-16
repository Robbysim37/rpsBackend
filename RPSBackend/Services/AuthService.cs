using System.Net.Http.Headers;
using Google.Apis.Auth;

namespace RpsBackend.Services;

public class AuthService
{
    private readonly IConfiguration _config;

    public AuthService(IConfiguration config)
    {
        _config = config;
    }

    public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleIdAsync(string idToken)
    {
        if (string.IsNullOrWhiteSpace(idToken))
        {
            throw new ArgumentException("Missing idToken");
        }

        var googleClientId = _config["GoogleAuth:ClientId"];
        Console.WriteLine($"GoogleAuth:ClientId = '{googleClientId}'");
        
        if (string.IsNullOrWhiteSpace(googleClientId))
        {
            throw new InvalidOperationException("Google client ID is not configured");
        }

        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(
                idToken,
                new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { googleClientId }
                });

            return payload;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Invalid Google ID token: {ex.Message}");
            throw new UnauthorizedAccessException("Invalid Google ID token");
        }
    }

    public async Task<(string Sub, string Name, string Picture)> GetGoogleUserInfoAsync(string accessToken)
    {
        if (string.IsNullOrWhiteSpace(accessToken))
            throw new ArgumentException("Missing accessToken");

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var res = await client.GetAsync("https://openidconnect.googleapis.com/v1/userinfo");
        if (!res.IsSuccessStatusCode)
            throw new UnauthorizedAccessException("Invalid Google access token");

        var json = await res.Content.ReadAsStringAsync();

        // minimal parse without extra libs:
        using var doc = System.Text.Json.JsonDocument.Parse(json);
        var root = doc.RootElement;

        var sub = root.GetProperty("sub").GetString() ?? throw new UnauthorizedAccessException("Missing sub");
        var name = root.TryGetProperty("name", out var n) ? n.GetString() ?? "" : "";
        var picture = root.TryGetProperty("picture", out var p) ? p.GetString() ?? "" : "";

        return (sub, name, picture);
    }
}
