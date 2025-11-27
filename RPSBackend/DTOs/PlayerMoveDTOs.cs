using RpsBackend.Models;
namespace RpsBackend.DTOs
{
    public class PlayRequestDto
    {
        public Move HumanMove { get; set; }
    }

    public class PlayResponseDto
    {
        public Move AIMove { get; set; }
        public Result Winner { get; set; }
    }

    public class SessionMoveHistory
    {
        // List of players moves and results
    }

    public class AlgorithmTestingResultsDto
    {
        public required int numberOfGames {get; init;}

        public double wins { get; set; } = 0;

        public double ties { get; set; } = 0;

        public double losses { get; set; } = 0;
    }
}