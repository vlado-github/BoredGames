using BoredGames.Server.Common.Enums;
using BoredGames.Server.Common.Exceptions;
using Orleans;

namespace BoredGames.Server.Domain.Grains;

public class GameGrain : Grain, IGameGrain
{
    private List<Guid> _playersIds;
    private GameState _gameState;
    private Guid _winnerId;

    public override Task OnActivateAsync(CancellationToken token)
    {
        _playersIds = new List<Guid>();
        _gameState = GameState.AwaitingPlayers;
        _winnerId = Guid.Empty;

        return base.OnActivateAsync(token);
    }

    public Task<GameState> AddPlayerToGame(Guid playerId)
    {
        if (_gameState is GameState.Finished)
        {
            throw new ActionValidationException("Player can't joined to finished game.");
        }
        
        if (_gameState is GameState.InPlay)
        {
            throw new ActionValidationException("Player can't joined during play.");
        }
        
        _playersIds.Add(playerId);

        if (_gameState is GameState.AwaitingPlayers && _playersIds.Count == 2)
        {
            _gameState = GameState.InPlay;
        }

        return Task.FromResult(_gameState);
    }
}