namespace RpsBackend.DTOs
{
    public class PlayRequestDto
    {
        // "R", "P", or "S"
        public string HumanMove { get; set; } = string.Empty;

        // Optional - frontend can send it or leave null
        public string? Location { get; set; }

        // Optional - can be Google ID later
        public string? UserId { get; set; }
    }

    public class PlayResponseDto
    {
        public string AIMove { get; set; } = string.Empty;

        public string Winner { get; set; } = string.Empty;
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