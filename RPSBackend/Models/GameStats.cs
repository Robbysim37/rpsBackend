namespace RpsBackend.Models
{
    public class AnonymousGame
    {
        public int Id { get; set; }  

        public Move HumanMove { get; set; }
        public Move AiMove { get; set; }
        public Result HumansResult { get; set; }
    }
    public class UserGame
    {
        public int Id { get; set; }

        // Foreign key to User
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public Move HumanMove { get; set; }
        public Move AiMove { get; set; }
        public Result HumansResult { get; set; }
    }
}