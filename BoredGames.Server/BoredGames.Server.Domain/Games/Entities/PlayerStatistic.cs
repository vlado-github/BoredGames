using Orleans;

namespace BoredGames.Server.Domain.Games.Entities;

[GenerateSerializer]
public class PlayerStatistic
{
    public PlayerStatistic(Player player)
    {
        PlayerId = player.Id;
        PlayerNickName = player.NickName;
        RoundWins = new List<Hand>();
        RoundLosses = new List<Hand>();
        RoundDraws = new List<Hand>();
    }

    public void AddWin(int roundNumber, string actionType)
    {
        RoundWins.Add(new Hand(roundNumber, actionType));
    }
    
    public void AddLoss(int roundNumber, string actionType)
    {
        RoundLosses.Add(new Hand(roundNumber, actionType));
    }
    
    public void AddDraw(int roundNumber, string actionType)
    {
        RoundDraws.Add(new Hand(roundNumber, actionType));
    }
    
    [Id(0)]
    public Guid PlayerId { get; private set; }
    [Id(1)]
    public  IList<Hand> RoundWins { get; private set; }
    [Id(2)]
    public IList<Hand> RoundLosses { get; private set; }
    [Id(3)]
    public IList<Hand> RoundDraws { get; private set; }
    [Id(4)]
    public string PlayerNickName { get; private set; }
    public int NumberOfWins => RoundWins.Count;
    public int NumberOfLoses => RoundLosses.Count;
    public int NumberOfDraws => RoundDraws.Count;
}