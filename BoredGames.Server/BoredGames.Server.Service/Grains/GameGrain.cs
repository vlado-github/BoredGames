using BoredGames.Server.Common.Enums;
using BoredGames.Server.Common.Exceptions;
using BoredGames.Server.Domain.Games.Base;
using BoredGames.Server.Domain.Games.Dtos;
using BoredGames.Server.Domain.Games.Entities;
using BoredGames.Server.Service.Commands;
using BoredGames.Server.Service.Grains.Base;
using BoredGames.Server.Service.ViewModels;
using Mapster;
using Orleans;

namespace BoredGames.Server.Service.Grains;

public class GameGrain : Grain, IGameGrain
{
    private IList<PlayerDto> _players;
    private GameState _gameState;
    private IGameRuleEngine _gameRuleEngine;

    public override Task OnActivateAsync(CancellationToken token)
    {
        _players = new List<PlayerDto>();
        _gameState = new GameState
        {
            GameId = this.GetPrimaryKey(),
            GameStatus = GameStatus.AwaitingPlayers
        };
        return base.OnActivateAsync(token);
    }

    public void Setup(CreateGameCommand command)
    {
        var dto = command.Adapt<GameDto>();
        _gameRuleEngine = GameRuleEngineFactory.GetInstance(dto);
        
        var roundResult = _gameRuleEngine.GetCurrentRoundResult();
        _gameState.RoundNumber = roundResult.RoundNumber;
        _gameState.RoundStatus = roundResult.RoundStatus;
    }

    public Task<GameDefinitionViewModel> AddPlayerToGame(AddPlayerCommand command)
    {
        if (_gameState.GameStatus is GameStatus.Finished)
        {
            throw new OperationNotAllowedException("Player can't joined to finished game.");
        }
        
        if (_gameState.GameStatus is GameStatus.InPlay)
        {
            throw new OperationNotAllowedException("Player can't joined during play.");
        }

        var dto = command.Adapt<PlayerDto>();
        if (!_players.Select(x => x.Id).Contains(dto.Id))
        {
            _players.Add(dto);
        }

        if (_gameState.GameStatus is GameStatus.AwaitingPlayers 
            && _players.Count == _gameRuleEngine.GetConfiguration().RequiredNumberOfPlayers)
        {
            _gameState.GameStatus = GameStatus.InPlay;
        }

        var settings = _gameRuleEngine.GetConfiguration();

        var result = settings.Adapt<GameDefinitionViewModel>();
        result.GameId = this.GetPrimaryKey();
        return Task.FromResult(result);
    }

    public Task<GameStateViewModel> MakeMove(MakeMoveCommand command)
    {
        var dto = command.Adapt<MoveDto>();
        var result = _gameRuleEngine.Handle(dto);
        _gameState.RoundNumber = result.RoundNumber;
        _gameState.RoundStatus = result.RoundStatus;
        
        // Game ends if all rounds are completed or 
        // required number of wins in match is met.
        var allRoundsFinished = _gameRuleEngine.AreAllRoundsFinished();
        var score = _gameRuleEngine.GetScore();
        if (allRoundsFinished || score.IsRequiredNumberOfWinsMet())
        {
            _gameState.GameStatus = GameStatus.Finished;
        }
        else
        {
            _gameState.GameStatus = GameStatus.InPlay;
        }

        return Task.FromResult(_gameState.Adapt<GameStateViewModel>());
    }

    public Task<GameScoreViewModel> GetScore()
    {
        var result = _gameRuleEngine.GetScore();
        return Task.FromResult(result.Adapt<GameScoreViewModel>());
    }

    public Task<GameDefinitionViewModel> GetConfig()
    {
        var config = _gameRuleEngine.GetConfiguration();
        var definition = config.Adapt<GameDefinitionViewModel>();
        definition.GameId = this.GetPrimaryKey();
        return Task.FromResult(definition);
    }

    public Task<IList<PlayerViewModel>> GetWinners()
    {
        var result = _gameRuleEngine.GetWinners();
        return Task.FromResult(result.Adapt<IList<PlayerViewModel>>());
    }

    public Task<GameStateViewModel> GetState()
    {
        return Task.FromResult(_gameState.Adapt<GameStateViewModel>());
    }
}