namespace BoredGames.Client.CLI.API.Responses;

public class GameScoreResponse
{
    public int CurrentRound { get; set; }
    public int RequiredNumberOfWins { get; set; }
    public IList<PlayerScore> PlayerScores { get; set; }
}

public class PlayerScore
{
    public Guid PlayerId { get; set; }
    public IList<RoundResult> RoundWins { get; set; }
    public IList<RoundResult> RoundLosses { get; set; }
}

public class RoundResult
{
    public int RoundNumber { get; set; }
    public string PlayerMove { get; set; }
}