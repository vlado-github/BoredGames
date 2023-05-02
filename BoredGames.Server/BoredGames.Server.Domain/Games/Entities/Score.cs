namespace BoredGames.Server.Domain.Games.Entities;

public class Score
{
    private readonly int _requiredNumberOfWins;
    private readonly IList<Statistic> _playerStats;

    public Score(int requiredNumberOfWins)
    {
        _requiredNumberOfWins = requiredNumberOfWins;
        _playerStats = new List<Statistic>();
    }

    public void AddWin(Guid playerId, int roundNumber, string actionType)
    {
        var playerStats = _playerStats.SingleOrDefault(x => x.PlayerId == playerId);
        if (playerStats == null)
        {
            playerStats = new Statistic(playerId);
            playerStats.AddWin(roundNumber, actionType);
            _playerStats.Add(playerStats);
        }
        else
        {
            playerStats.AddLoss(roundNumber, actionType);
        }
    }

    public void AddLoss(Guid playerId, int roundNumber, string actionType)
    {
        var playerStats = _playerStats.SingleOrDefault(x => x.PlayerId == playerId);
        if (playerStats == null)
        {
            playerStats = new Statistic(playerId);
            playerStats.AddLoss(roundNumber, actionType);
            _playerStats.Add(playerStats);
        }
        else
        {
            playerStats.AddLoss(roundNumber, actionType);
        }
    }

    public IList<Guid> GetWinners()
    {
        return _playerStats
            .Where(x => x.NumberOfWins == _requiredNumberOfWins)
            .Select(x => x.PlayerId)
            .ToList();
    }

    public IList<Statistic> GetScore()
    {
        return _playerStats;
    }
}