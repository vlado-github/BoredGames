using BoredGames.Server.Common.Enums;
using BoredGames.Server.Common.Exceptions;
using BoredGames.Server.Domain.Commands;
using BoredGames.Server.Domain.Games.Base;
using BoredGames.Server.Domain.Games.Entities;
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
        return base.OnActivateAsync(token);
    }

    public void Setup(CreateGameCommand command)
    {
        _gameRuleEngine = GameRuleEngineFactory.GetInstance(command);
    }

    public Task<GameDefinition> AddPlayerToGame(Guid playerId)
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

        if (_gameState is GameState.AwaitingPlayers 
            && _playersIds.Count == _gameRuleEngine.GetConfiguration().RequiredNumberOfPlayers)
        {
            _gameState = GameState.InPlay;
        }

        var settings = _gameRuleEngine.GetConfiguration();
        var result = new GameDefinition
        {
            GameId = this.GetPrimaryKey(),
            State = _gameState,
            RequiredNumberOfPlayers = settings.RequiredNumberOfPlayers,
            RequiredNumberOfWins = settings.RequiredNumberOfWins,
            Description = settings.Description
        };

        return Task.FromResult(result);
    }

    public Task<GameState> MakeMove(MakeMoveCommand command)
    {
        var result = _gameRuleEngine.Handle(command);
        _gameState = result;
        return Task.FromResult(result);
    }

    public Task<GameScore> GetScore()
    {
        var result = _gameRuleEngine.GetScore();
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