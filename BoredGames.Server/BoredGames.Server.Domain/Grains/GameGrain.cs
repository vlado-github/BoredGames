using BoredGames.Server.Common.Enums;
using BoredGames.Server.Common.Exceptions;
using BoredGames.Server.Domain.Commands;
using BoredGames.Server.Domain.Games.Base;
using BoredGames.Server.Domain.Games.RockPaperSissors;
using BoredGames.Server.Domain.Grains.Base;
using Orleans;

namespace BoredGames.Server.Domain.Grains;

public class GameGrain : Grain, IGameGrain
{
    private List<Guid> _playersIds;
    private GameState _gameState;
    private IGameRuleEngine _gameRuleEngine;

    public override Task OnActivateAsync(CancellationToken token)
    {
        _playersIds = new List<Guid>();
        _gameState = GameState.AwaitingPlayers;
        _gameRuleEngine = new RockPaperSissorsRuleEngine(1, 2);
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

        if (!_playersIds.Contains(playerId))
        {
            _playersIds.Add(playerId);
        }

        if (_gameState is GameState.AwaitingPlayers && _playersIds.Count == 2)
        {
            _gameState = GameState.InPlay;
        }

        return Task.FromResult(_gameState);
    }

    public Task<GameState> MakeMove(MakeMoveCommand command)
    {
        var result = _gameRuleEngine.Handle(command);
        _gameState = result;
        return Task.FromResult(result);
    }

    public Task<IList<Guid>> GetWinners()
    {
        var result = _gameRuleEngine.GetWinners();
        return Task.FromResult(result);
    }

    public Task<GameState> GetState()
    {
        return Task.FromResult(_gameState);
    }
}