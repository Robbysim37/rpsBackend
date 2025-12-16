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
}
