using Orleans;

namespace BoredGames.Server.Domain.Games.Entities;

[GenerateSerializer]
public class GameScore
{
    [Id(0)]
    public int RequiredNumberOfWins { get; private set; }
    [Id(1)]
    public IList<PlayerStatistic> PlayersStats { get; private set; }
    [Id(2)]
    public int CurrentRoundNumber { get; private set; }

    public GameScore(int requiredNumberOfWins)
    {
        RequiredNumberOfWins = requiredNumberOfWins;
        PlayersStats = new List<PlayerStatistic>();
        CurrentRoundNumber = 1;
    }

    public void AddWin(Guid playerId, int roundNumber, string actionType)
    {
        CurrentRoundNumber = roundNumber;
        var playerStat = PlayersStats.SingleOrDefault(x => x.PlayerId == playerId);
        if (playerStat == null)
        {
            playerStat = new PlayerStatistic(playerId);
            playerStat.AddWin(roundNumber, actionType);
            PlayersStats.Add(playerStat);
        }
        else
        {
            playerStat.AddLoss(roundNumber, actionType);
        }
    }

    public void AddLoss(Guid playerId, int roundNumber, string actionType)
    {
        CurrentRoundNumber = roundNumber;
        var playerStat = PlayersStats.SingleOrDefault(x => x.PlayerId == playerId);
        if (playerStat == null)
        {
            playerStat = new PlayerStatistic(playerId);
            playerStat.AddLoss(roundNumber, actionType);
            PlayersStats.Add(playerStat);
        }
        else
        {
            playerStat.AddLoss(roundNumber, actionType);
        }
    }
    
    public void AddDraw(Guid playerId, int roundNumber, string actionType)
    {
        CurrentRoundNumber = roundNumber;
        var playerStat = PlayersStats.SingleOrDefault(x => x.PlayerId == playerId);
        if (playerStat == null)
        {
            playerStat = new PlayerStatistic(playerId);
            playerStat.AddDraw(roundNumber, actionType);
            PlayersStats.Add(playerStat);
        }
        else
        {
            playerStat.AddLoss(roundNumber, actionType);
        }
    }

    public IList<Guid> GetWinners()
    {
        return PlayersStats
            .Where(x => x.NumberOfWins == RequiredNumberOfWins)
            .Select(x => x.PlayerId)
            .ToList();
    }
}