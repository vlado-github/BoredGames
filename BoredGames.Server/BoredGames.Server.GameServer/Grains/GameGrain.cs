using BoredGames.Common.Enums;
using BoredGames.Server.Domain.Games.Base;
using BoredGames.Server.Domain.Games.Dtos;
using BoredGames.Server.Domain.Games.Entities;
using BoredGames.Server.Domain.Games.PubSub;
using BoredGames.Server.GameServer.Commands;
using BoredGames.Server.GameServer.Grains.Base;
using BoredGames.Server.GameServer.ViewModels;
using Mapster;

namespace BoredGames.Server.GameServer.Grains;

public class GameGrain : Grain, IGameGrain
{
    private GameState _gameState;
    private IGameRuleEngine _gameRuleEngine;
    private GameStateTracker _gameStateTracker;

    public override Task OnActivateAsync(CancellationToken token)
    {
        _gameState = new GameState(this.GetPrimaryKey(), GameStatus.AwaitingPlayers);
        _gameStateTracker =  new GameStateTracker();
        _gameRuleEngine = GameRuleEngineFactory.GetInstance(GameDto.Default);
        _gameStateTracker.Subscribe(_gameRuleEngine);
        return base.OnActivateAsync(token);
    }

    public void Setup(CreateGameCommand command)
    {
        var dto = command.Adapt<GameDto>();
        _gameStateTracker =  new GameStateTracker();
        _gameRuleEngine = GameRuleEngineFactory.GetInstance(dto);
        _gameStateTracker.Subscribe(_gameRuleEngine);
        
        var roundResult = _gameRuleEngine.GetCurrentRoundResult();
        _gameState.SyncRoundResult(roundResult);
    }

    public void AddPlayerToGame(AddPlayerCommand command)
    {
        if (_gameState.PlayersNumber == _gameRuleEngine.GetDefinition().RequiredNumberOfPlayers)
        {
            return;
        }
        
        var dto = new PlayerDto()
        {
            Id = command.Id,
            NickName = command.NickName,
        };
        if (_gameState.Players.All(x => x.Id != dto.Id))
        {
            _gameState.Players.Add(dto);
        }

        if (_gameState.GameStatus is GameStatus.AwaitingPlayers 
            && _gameState.PlayersNumber == _gameRuleEngine.GetDefinition().RequiredNumberOfPlayers)
        {
            _gameState.ChangeGameStatus(GameStatus.InPlay, _gameStateTracker);
        }
    }

    public Task<GameStateViewModel> MakeMove(MakeMoveCommand command)
    {
        var dto = new MoveDto()
        {
            PlayerId = command.PlayerId,
            PlayerNickName = command.PlayerNickName,
            ActionType = command.ActionType,
            SelectedTile = new TilePosition(command.SelectedTileRow, command.SelectedTileColumn)
        };
        var result = _gameRuleEngine.Handle(dto);
        _gameState.SyncRoundResult(result);
        
        // Game ends if all rounds are completed or 
        // required number of wins in match is met.
        var allRoundsFinished = _gameRuleEngine.AreAllRoundsFinished();
        var score = _gameRuleEngine.GetScore();
        if (allRoundsFinished || score.IsRequiredNumberOfWinsMet())
        {
            _gameState.ChangeGameStatus(GameStatus.Finished, _gameStateTracker);
        }
        else
        {
            _gameState.ChangeGameStatus(GameStatus.InPlay, _gameStateTracker);
        }

        var newGameState = _gameState.Adapt<GameStateViewModel>();
        newGameState.Score = new GameScoreViewModel()
        {
            LastRound = score.LastRoundNumber,
            RequiredNumberOfWins = score.RequiredNumberOfWins,
            PlayerScores = score.PlayerStatistics.Adapt<List<PlayerScoreViewModel>>()
        };

        return Task.FromResult(newGameState);
    }

    public Task<GameScoreViewModel> GetScore()
    {
        var result = _gameRuleEngine.GetScore();
        return Task.FromResult(result.Adapt<GameScoreViewModel>());
    }

    public Task<GameDefinitionViewModel> GetDefinition()
    {
        var config = _gameRuleEngine.GetDefinition();
        var definition = config.Adapt<GameDefinitionViewModel>();
        definition.GameId = this.GetPrimaryKey();
        return Task.FromResult(definition);
    }

    public Task<GameWinnersViewModel> GetWinners()
    {
        var result = _gameRuleEngine.GetWinners();
        return Task.FromResult(new GameWinnersViewModel()
        {
            Winners = result.Adapt<IList<PlayerViewModel>>()
        });
    }

    public async Task<GameStateViewModel> GetState()
    {
        var gameState = new GameStateViewModel()
        {
            GameId = _gameState.GameId,
            GameStatus = _gameState.GameStatus,
            RoundStatus =  _gameState.RoundStatus,
            RoundNumber =  _gameState.RoundNumber,
            PlayersNumber =  _gameState.PlayersNumber,
            PlayersTurnOrder =  _gameState.PlayersTurnOrder,
            CurrentPlayerTurn =  _gameState.CurrentPlayerTurn,
            Players = _gameState.Players.Select(x => new PlayerViewModel()
            {
                Id = x.Id,
                NickName = x.NickName
            }).ToList(),
            Moves = _gameState.Moves.Select(x => new MoveViewModel()
            {
                ActionType = x.ActionType,
                PlayerId = x.PlayerId,
                PlayerNickName = x.PlayerNickName,
                SelectedTile = new TilePositionViewModel()
                {
                    Column = x.SelectedTile.Column,
                    Row = x.SelectedTile.Row
                }
            }).ToList()
        };
        gameState.Score = await GetScore();
        return gameState;
    }
}