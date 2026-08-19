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
    private readonly IPersistentState<GameState> _gameState;
    private IGameRuleEngine _gameRuleEngine;
    private GameStateTracker _gameStateTracker;

    public GameGrain(
        [PersistentState("gameState", "Default")] IPersistentState<GameState> gameState)
    {
        _gameState = gameState;
        _gameStateTracker = new GameStateTracker();
        _gameRuleEngine = GameRuleEngineFactory.GetInstance(GameDto.Default);
        _gameStateTracker.Subscribe(_gameRuleEngine);
    }

    public override async Task OnActivateAsync(CancellationToken token)
    {
        _gameState.State.GameId = this.GetPrimaryKey();
        await _gameState.WriteStateAsync(token);
        await base.OnActivateAsync(token);
    }

    public async Task Setup(CreateGameCommand command)
    {
        var dto = command.Adapt<GameDto>();
        _gameStateTracker = new GameStateTracker();
        _gameRuleEngine = GameRuleEngineFactory.GetInstance(dto);
        _gameStateTracker.Subscribe(_gameRuleEngine);
        
        var roundResult = _gameRuleEngine.GetCurrentRoundResult();
        _gameState.State.SyncRoundResult(roundResult);
        await _gameState.WriteStateAsync();
    }

    public async Task AddPlayerToGame(AddPlayerCommand command)
    {
        if (_gameState.State.PlayersNumber == _gameRuleEngine.GetDefinition().RequiredNumberOfPlayers)
        {
            return;
        }
        
        var dto = new PlayerDto()
        {
            Id = command.Id,
            NickName = command.NickName,
        };
        if (_gameState.State.Players.All(x => x.Id != dto.Id))
        {
            _gameState.State.Players.Add(dto);
        }

        if (_gameState.State.GameStatus is GameStatus.AwaitingPlayers 
            && _gameState.State.PlayersNumber == _gameRuleEngine.GetDefinition().RequiredNumberOfPlayers)
        {
            _gameState.State.ChangeGameStatus(GameStatus.InPlay, _gameStateTracker);
            var roundResult = _gameRuleEngine.GetCurrentRoundResult();
            _gameState.State.SyncRoundResult(roundResult);
        }
        
        await _gameState.WriteStateAsync();
    }

    public async Task<GameStateViewModel> MakeMove(MakeMoveCommand command)
    {
        var dto = new MoveDto()
        {
            PlayerId = command.PlayerId,
            PlayerNickName = command.PlayerNickName,
            ActionType = command.ActionType,
            SelectedTile = new TilePosition(command.SelectedTileRow, command.SelectedTileColumn)
        };
        var result = _gameRuleEngine.Handle(dto);
        _gameState.State.SyncRoundResult(result);
        
        // Game ends if all rounds are completed or 
        // required number of wins in match is met.
        var allRoundsFinished = _gameRuleEngine.AreAllRoundsFinished();
        var score = _gameRuleEngine.GetScore();
        if (allRoundsFinished || score.IsRequiredNumberOfWinsMet())
        {
            _gameState.State.ChangeGameStatus(GameStatus.Finished, _gameStateTracker);
        }
        else
        {
            _gameState.State.ChangeGameStatus(GameStatus.InPlay, _gameStateTracker);
        }

        var newGameState = await GetState();
        newGameState.Score = new GameScoreViewModel()
        {
            LastRound = score.LastRoundNumber,
            RequiredNumberOfWins = score.RequiredNumberOfWins,
            PlayerScores = score.PlayerStatistics.Adapt<List<PlayerScoreViewModel>>()
        };
        
        await _gameState.WriteStateAsync();

        return newGameState;
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
            GameId = _gameState.State.GameId,
            GameStatus = _gameState.State.GameStatus,
            RoundStatus =  _gameState.State.RoundStatus,
            RoundNumber =  _gameState.State.RoundNumber,
            PlayersNumber =  _gameState.State.PlayersNumber,
            PlayersTurnOrder =  _gameState.State.PlayersTurnOrder,
            CurrentPlayerTurn =  _gameState.State.CurrentPlayerTurn,
            Players = _gameState.State.Players.Select(x => new PlayerViewModel()
            {
                Id = x.Id,
                NickName = x.NickName
            }).ToList(),
            Moves = _gameState.State.Moves.Select(x => new MoveViewModel()
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