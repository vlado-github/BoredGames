namespace BoredGames.Server.Domain.Games.Entities;

public class Statistic
{
    private readonly int _requiredNumberOfWins;
    private readonly IDictionary<Guid, int> _playerWins;

    public Statistic(int requiredNumberOfWins)
    {
        _requiredNumberOfWins = requiredNumberOfWins;
        _playerWins = new Dictionary<Guid, int>();
    }

    public void AddWin(Guid playerId)
    {
        _playerWins.TryGetValue(playerId, out var currentNumberOfWins);
        _playerWins.Add(playerId, ++currentNumberOfWins);
    }

    public IList<Guid> GetWinners()
    {
        return _playerWins
            .Where(x => x.Value == _requiredNumberOfWins)
            .Select(x => x.Key)
            .ToList();
    }
}