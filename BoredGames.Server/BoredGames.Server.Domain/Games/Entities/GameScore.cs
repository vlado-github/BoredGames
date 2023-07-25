using Orleans;

namespace BoredGames.Server.Domain.Games.Entities;

[GenerateSerializer]
public class GameScore
{
    [Id(0)]
    public IList<PlayerStatistic> PlayerStatistics { get; private set; }
    [Id(1)]
    public int CurrentRoundNumber { get; private set; }

    private readonly int _requiredNumberOfWins;

    public GameScore(int requiredNumberOfWins)
    {
        PlayerStatistics = new List<PlayerStatistic>();
        CurrentRoundNumber = 1;
        _requiredNumberOfWins = requiredNumberOfWins;
    }

    public void AddWin(Player player, int roundNumber, string actionType)
    {
        CurrentRoundNumber = roundNumber;
        var playerStat = PlayerStatistics.SingleOrDefault(x => x.PlayerId == player.Id);
        if (playerStat == null)
        {
            playerStat = new PlayerStatistic(player);
            playerStat.AddWin(roundNumber, actionType);
            PlayerStatistics.Add(playerStat);
        }
        else
        {
            playerStat.AddWin(roundNumber, actionType);
        }
    }

    public void AddLoss(Player player, int roundNumber, string actionType)
    {
        CurrentRoundNumber = roundNumber;
        var playerStat = PlayerStatistics.SingleOrDefault(x => x.PlayerId == player.Id);
        if (playerStat == null)
        {
            playerStat = new PlayerStatistic(player);
            playerStat.AddLoss(roundNumber, actionType);
            PlayerStatistics.Add(playerStat);
        }
        else
        {
            playerStat.AddLoss(roundNumber, actionType);
        }
    }
    
    public void AddDraw(Player player, int roundNumber, string actionType)
    {
        CurrentRoundNumber = roundNumber;
        var playerStat = PlayerStatistics.SingleOrDefault(x => x.PlayerId == player.Id);
        if (playerStat == null)
        {
            playerStat = new PlayerStatistic(player);
            playerStat.AddDraw(roundNumber, actionType);
            PlayerStatistics.Add(playerStat);
        }
        else
        {
            playerStat.AddDraw(roundNumber, actionType);
        }
    }

    public IList<Player> GetWinners()
    {
        var winners = PlayerStatistics
            .Where(x => x.NumberOfWins == _requiredNumberOfWins)
            .Select(x => new Player(x.PlayerId, x.PlayerNickName))
            .ToList();
        return winners;
    }
}