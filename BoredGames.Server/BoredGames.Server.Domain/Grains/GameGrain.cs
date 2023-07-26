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
    private IList<Player> _players;
    private GameState _gameState;
    private IGameRuleEngine _gameRuleEngine;

    public override Task OnActivateAsync(CancellationToken token)
    {
        _players = new List<Player>();
        _gameState = new GameState
        {
            GameId = this.GetPrimaryKey(),
            GameStatus = GameStatus.AwaitingPlayers
        };
        return base.OnActivateAsync(token);
    }

    public void Setup(CreateGameCommand command)
    {
        _gameRuleEngine = GameRuleEngineFactory.GetInstance(command);
        
        var roundResult = _gameRuleEngine.GetCurrentRoundResult();
        _gameState.RoundNumber = roundResult.RoundNumber;
        _gameState.RoundStatus = roundResult.RoundStatus;
    }

    public Task<GameDefinition> AddPlayerToGame(Player player)
    {
        if (_gameState.GameStatus is GameStatus.Finished)
        {
            throw new ActionValidationException("Player can't joined to finished game.");
        }
        
        if (_gameState.GameStatus is GameStatus.InPlay)
        {
            throw new ActionValidationException("Player can't joined during play.");
        }

        if (!_players.Select(x => x.Id).Contains(player.Id))
        {
            _players.Add(player);
        }

        if (_gameState.GameStatus is GameStatus.AwaitingPlayers 
            && _players.Count == _gameRuleEngine.GetConfiguration().RequiredNumberOfPlayers)
        {
            _gameState.GameStatus = GameStatus.InPlay;
        }

        var settings = _gameRuleEngine.GetConfiguration();
        var result = new GameDefinition
        {
            GameId = this.GetPrimaryKey(),
            RequiredNumberOfPlayers = settings.RequiredNumberOfPlayers,
            RequiredNumberOfWins = settings.RequiredNumberOfWins,
            Description = settings.Description
        };

        return Task.FromResult(result);
    }

    public Task<RoundResult> MakeMove(MakeMoveCommand command)
    {
        var result = _gameRuleEngine.Handle(command);
        _gameState.RoundNumber = result.RoundNumber;
        _gameState.RoundStatus = result.RoundStatus;
        
        var allRoundsFinished = _gameRuleEngine.AreAllRoundsFinished();
        if (allRoundsFinished)
        {
            _gameState.GameStatus = GameStatus.Finished;
        }
        else
        {
            _gameState.GameStatus = GameStatus.InPlay;
        }
        
        return Task.FromResult(result);
    }

    public Task<GameScore> GetScore()
    {
        var result = _gameRuleEngine.GetScore();
        return Task.FromResult(result);
    }

    public Task<IList<Player>> GetWinners()
    {
        var result = _gameRuleEngine.GetWinners();
        return Task.FromResult(result);
    }

    public Task<GameState> GetState()
    {
        return Task.FromResult(_gameState);
    }
}