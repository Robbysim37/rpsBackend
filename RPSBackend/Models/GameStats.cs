namespace RpsBackend.Models
{
    public class AnonymousGame
    {
        public int Id { get; set; }  // 👈 primary key required for EF

        public Move HumanMove { get; set; }
        public Move AiMove { get; set; }
        public Result HumansResult { get; set; }
    }
}