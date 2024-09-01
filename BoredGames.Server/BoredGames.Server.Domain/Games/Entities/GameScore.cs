namespace BoredGames.Server.Domain.Games.Entities;

public class GameScore
{
    public IList<PlayerStatistic> PlayerStatistics { get; private set; }
    public int CurrentRoundNumber { get; private set; }

    public readonly int RequiredNumberOfWins;
    public readonly int NumberOfRounds;

    public GameScore(int numberOfRounds, int requiredNumberOfWins)
    {
        PlayerStatistics = new List<PlayerStatistic>();
        CurrentRoundNumber = 1;
        RequiredNumberOfWins = requiredNumberOfWins;
        NumberOfRounds = numberOfRounds;
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

    public bool IsRequiredNumberOfWinsMet()
    {
        if (!PlayerStatistics.Any())
        {
            return false;
        }
        var playerMaxWins = PlayerStatistics.Max(x => x.NumberOfWins);
        var playerMinWins = PlayerStatistics.Min(x => x.NumberOfWins);
        return (playerMaxWins - playerMinWins) >= RequiredNumberOfWins;
    }

    public IList<Player> GetWinners()
    {
        var maxWins = PlayerStatistics
            .Max(x => x.NumberOfWins);
        
        if (maxWins == 0)
        {
            return new List<Player>();
        }
        
        var winners = PlayerStatistics
            .Where(x => x.NumberOfWins == maxWins)
            .Select(x => new Player(x.PlayerId, x.PlayerNickName))
            .ToList();
        
        return winners;
    }
}