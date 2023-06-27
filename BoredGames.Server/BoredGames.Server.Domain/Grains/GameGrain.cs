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
    private GameStatus _gameStatus;
    private IGameRuleEngine _gameRuleEngine;

    public override Task OnActivateAsync(CancellationToken token)
    {
        _playersIds = new List<Guid>();
        _gameStatus = GameStatus.AwaitingPlayers;
        return base.OnActivateAsync(token);
    }

    public void Setup(CreateGameCommand command)
    {
        _gameRuleEngine = GameRuleEngineFactory.GetInstance(command);
    }

    public Task<GameDefinition> AddPlayerToGame(Guid playerId)
    {
        if (_gameStatus is GameStatus.Finished)
        {
            throw new ActionValidationException("Player can't joined to finished game.");
        }
        
        if (_gameStatus is GameStatus.InPlay)
        {
            throw new ActionValidationException("Player can't joined during play.");
        }

        if (!_playersIds.Contains(playerId))
        {
            _playersIds.Add(playerId);
        }

        if (_gameStatus is GameStatus.AwaitingPlayers 
            && _playersIds.Count == _gameRuleEngine.GetConfiguration().RequiredNumberOfPlayers)
        {
            _gameStatus = GameStatus.InPlay;
        }

        var settings = _gameRuleEngine.GetConfiguration();
        var result = new GameDefinition
        {
            GameId = this.GetPrimaryKey(),
            Status = _gameStatus,
            RequiredNumberOfPlayers = settings.RequiredNumberOfPlayers,
            RequiredNumberOfWins = settings.RequiredNumberOfWins,
            Description = settings.Description
        };

        return Task.FromResult(result);
    }

    public Task<RoundResult> MakeMove(MakeMoveCommand command)
    {
        var result = _gameRuleEngine.Handle(command);
        _gameStatus = result.GameStatus;
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
        var currentRoundResult = _gameRuleEngine.GetRoundResult();
        var result = new GameState
        {
            GameId = this.GetPrimaryKey(),
            GameStatus = _gameStatus,
            RoundStatus = currentRoundResult.RoundStatus,
            RoundNumber = currentRoundResult.RoundNumber
        };
        return Task.FromResult(result);
    }
}