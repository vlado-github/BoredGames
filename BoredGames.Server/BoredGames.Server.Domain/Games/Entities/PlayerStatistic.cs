namespace BoredGames.Server.Domain.Games.Entities;

public class PlayerStatistic
{
    public PlayerStatistic(Guid playerId)
    {
        PlayerId = playerId;
        RoundWins = new List<Hand>();
        RoundLosses = new List<Hand>();
    }

    public void AddWin(int roundNumber, string actionType)
    {
        RoundWins.Add(new Hand(roundNumber, actionType));
    }
    
    public void AddLoss(int roundNumber, string actionType)
    {
        RoundLosses.Add(new Hand(roundNumber, actionType));
    }
    
    public Guid PlayerId { get; private set; }
    public  IList<Hand> RoundWins { get; private set; }
    public IList<Hand> RoundLosses { get; private set; }
    public int NumberOfWins => RoundWins.Count;
    public int NumberOfLoses => RoundLosses.Count;
}