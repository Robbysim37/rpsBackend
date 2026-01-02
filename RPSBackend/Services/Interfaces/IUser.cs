using Google.Apis.Auth;
using RpsBackend.Models;

namespace RpsBackend.Services;

public interface IUser
{
    Task<User?> GetUserByIdAsync(int userId);

    // Existing (ID token / payload flow)
    Task<User> GetOrCreateFromGoogleAsync(GoogleJsonWebSignature.Payload payload);

    // ✅ New (Access token / userinfo flow)
    Task<User> GetOrCreateFromGoogleAsync(string googleSub, string name, string avatarUrl);
}
