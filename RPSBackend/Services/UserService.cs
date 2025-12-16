using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using RpsBackend.Data;
using RpsBackend.Models;

namespace RpsBackend.Services
{
    public class UserService : IUser
    {
        private readonly AppDbContext _db;

        public UserService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        // ✅ Existing method stays (ID token flow)
        public Task<User> GetOrCreateFromGoogleAsync(GoogleJsonWebSignature.Payload payload)
        {
            var googleSub = payload.Subject;
            var name = payload.Name ?? string.Empty;
            var avatar = payload.Picture ?? string.Empty;

            return GetOrCreateFromGoogleAsync(googleSub, name, avatar);
        }

        // ✅ New overload (access token -> userinfo flow)
        public async Task<User> GetOrCreateFromGoogleAsync(string googleSub, string name, string avatarUrl)
        {
            name ??= string.Empty;
            avatarUrl ??= string.Empty;

            // Look up existing user by GoogleId
            var user = await _db.Users
                .SingleOrDefaultAsync(u => u.GoogleId == googleSub);

            if (user is not null)
            {
                var changed = false;

                if (user.Name != name)
                {
                    user.Name = name;
                    changed = true;
                }

                if (user.AvatarUrl != avatarUrl)
                {
                    user.AvatarUrl = avatarUrl;
                    changed = true;
                }

                if (changed)
                {
                    await _db.SaveChangesAsync();
                }

                return user;
            }

            // No user yet: create a new one
            user = new User
            {
                GoogleId = googleSub,
                Name = name,
                AvatarUrl = avatarUrl,
                CreatedAt = DateTime.UtcNow
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return user;
        }
    }
}
