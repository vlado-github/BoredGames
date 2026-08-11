using BoredGames.Server.Domain.Games.Dtos;

namespace BoredGames.Server.Domain.Games.Entities;

public interface ITurn
{
    bool IsCurrent { get; }
    Guid PlayerId { get; }
    int Number { get; }
}

public class Turn : ITurn
{
    private readonly Guid _playerId;
    private readonly string _playerNickname;
    private readonly int _number;
    private bool _isCurrent;

    public Turn(Guid playerId, string playerNickname, int number = 1)
    {
        _playerId = playerId;
        _playerNickname = playerNickname;
        _number = number;
        _isCurrent = true;
    }

    public bool IsCurrent => _isCurrent;
    
    public Guid PlayerId => _playerId;
    
    public int Number => _number;

    public void ResetCurrent()
    {
        _isCurrent = false;
    }
}

