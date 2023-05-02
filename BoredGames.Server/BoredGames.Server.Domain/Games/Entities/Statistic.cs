namespace BoredGames.Server.Domain.Games.Entities;

public class Statistic
{
    public Statistic(Guid playerId)
    {
        PlayerId = playerId;
        RoundWins = new Dictionary<int, string>();
        RoundLosses = new Dictionary<int, string>();
    }

    public void AddWin(int roundNumber, string actionType)
    {
        RoundWins.Add(roundNumber, actionType);
    }
    
    public void AddLoss(int roundNumber, string actionType)
    {
        RoundLosses.Add(roundNumber, actionType);
    }
    
    public Guid PlayerId { get; private set; }
    private  IDictionary<int, string> RoundWins { get; set; }
    private IDictionary<int, string> RoundLosses { get; set; }
    public int NumberOfWins => RoundWins.Count;
    public int NumberOfLoses => RoundLosses.Count;
}