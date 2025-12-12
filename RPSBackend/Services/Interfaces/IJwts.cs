using RpsBackend.Models;

namespace RpsBackend.Services
{
    public interface IJwt
    {
        string GenerateToken(User user);
    }
}
