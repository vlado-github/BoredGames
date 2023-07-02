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

    public void AddWin(Guid playerId, int roundNumber, string actionType)
    {
        CurrentRoundNumber = roundNumber;
        var playerStat = PlayerStatistics.SingleOrDefault(x => x.PlayerId == playerId);
        if (playerStat == null)
        {
            playerStat = new PlayerStatistic(playerId);
            playerStat.AddWin(roundNumber, actionType);
            PlayerStatistics.Add(playerStat);
        }
        else
        {
            playerStat.AddWin(roundNumber, actionType);
        }
    }

    public void AddLoss(Guid playerId, int roundNumber, string actionType)
    {
        CurrentRoundNumber = roundNumber;
        var playerStat = PlayerStatistics.SingleOrDefault(x => x.PlayerId == playerId);
        if (playerStat == null)
        {
            playerStat = new PlayerStatistic(playerId);
            playerStat.AddLoss(roundNumber, actionType);
            PlayerStatistics.Add(playerStat);
        }
        else
        {
            playerStat.AddLoss(roundNumber, actionType);
        }
    }
    
    public void AddDraw(Guid playerId, int roundNumber, string actionType)
    {
        CurrentRoundNumber = roundNumber;
        var playerStat = PlayerStatistics.SingleOrDefault(x => x.PlayerId == playerId);
        if (playerStat == null)
        {
            playerStat = new PlayerStatistic(playerId);
            playerStat.AddDraw(roundNumber, actionType);
            PlayerStatistics.Add(playerStat);
        }
        else
        {
            playerStat.AddDraw(roundNumber, actionType);
        }
    }

    public IList<Guid> GetWinners()
    {
        var winners = PlayerStatistics
            .Where(x => x.NumberOfWins == _requiredNumberOfWins)
            .Select(x => x.PlayerId)
            .ToList();
        return winners;
    }
}