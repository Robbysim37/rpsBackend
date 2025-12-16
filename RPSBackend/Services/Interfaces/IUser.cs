using Google.Apis.Auth;
using RpsBackend.Models;

namespace RpsBackend.Services
{
    public interface IUser
    {
        Task<User> GetOrCreateFromGoogleAsync(GoogleJsonWebSignature.Payload payload);
        Task<User?> GetUserByIdAsync(int userId);
    }
}
