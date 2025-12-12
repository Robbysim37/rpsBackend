namespace RpsBackend.Models
{
    public class User
    {
        public int Id {get; set;}
        public string GoogleId { get; set;} = string.Empty;

        public List<UserGame> Games { get; set; } = new();
        public string Name { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}