using BoredGames.Server.Domain.Games.Dtos;

namespace BoredGames.Server.Domain.Games.Entities;

public class Turns
{
    private readonly IList<Turn> _turns;
    private readonly Guid[] _playersTurnOrder;
    private readonly IList<Player> _players;
    private static readonly Random RandomGenerator = new Random();

    public Turns(IList<Player> players)
    {
        _turns = new List<Turn>();
        _players = players;
        _playersTurnOrder = _players.Select(x => x.Id).OrderBy(_ => RandomGenerator.Next()).ToArray();
        _turns.Add(new Turn(_playersTurnOrder[0], _players.Single(x => x.Id == _playersTurnOrder[0]).NickName));
    }

    public ITurn Current => _turns.Single(x => x.IsCurrent);
    
    public Turn ShowNext(ITurn currentTurn) 
    {
        var currentIndex = _playersTurnOrder.IndexOf(currentTurn.PlayerId);
        var nextIndex = (currentIndex + 1) % _playersTurnOrder.Length;
        var nextPlayerId = _playersTurnOrder[nextIndex];
        var nextPlayer = _players.Single(x => x.Id == nextPlayerId);
        return new Turn(nextPlayer.Id, nextPlayer.NickName, currentTurn.Number + 1);
    }

    public void TurnNext()
    {
        if (_playersTurnOrder.Length == 0)
        {
            throw new InvalidOperationException("No players turn order available");
        }

        var currentTurn = Current;
        ResetCurrent();
        _turns.Add(ShowNext(currentTurn));
        
    }

    private void ResetCurrent()
    {
        foreach (var turn in _turns.Where(x => x.IsCurrent))
        {
            turn.ResetCurrent();
        }
    }
}
