using BoredGames.Server.API.Extensions;
using BoredGames.Server.API.Filters;
using BoredGames.Server.API.Models;
using BoredGames.Server.API.ViewModels;
using BoredGames.Server.Domain.Commands;
using BoredGames.Server.Domain.Games.RockPaperScissors;
using BoredGames.Server.Domain.Grains.Base;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace BoredGames.Server.API.Controllers
{
    [ApiKey]
    [Route("api/game")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGrainFactory _grainFactory;
        
        public GameController(IGrainFactory grainFactory)
        {
            _grainFactory = grainFactory;
        }
        
        [HttpPost("create")]
        public async Task<GameDefinitionViewModel> Create(CreateGame request)
        {
            var playerId = this.GetPlayerId();
            var player = _grainFactory.GetGrain<IPlayerGrain>(playerId);
            var gameDefinition = await player.CreateGame(new CreateGameCommand
            {
                Title = request.GameTitle,
                NumberOfPlayers = request.NumberOfPlayers,
                NumberOfWins = request.NumberOfWins,
                Description = request.Description
            });
            return gameDefinition.Adapt<GameDefinitionViewModel>();
        }

        [HttpPut("join")]
        public async Task<GameDefinitionViewModel> Join(JoinGame request)
        {
            var playerId = this.GetPlayerId();
            var player = _grainFactory.GetGrain<IPlayerGrain>(playerId);
            var gameDefinition = await player.JoinGame(new JoinGameCommand()
            {
                GameId = request.GameId
            });
            return gameDefinition.Adapt<GameDefinitionViewModel>();
        }

        [HttpPost("makemove")]
        public async Task<GameStateViewModel> MakeMove(MakeMove request)
        {
            var game = _grainFactory.GetGrain<IGameGrain<RockPaperScissorsSettings>>(request.GameId);
            var state = await game.MakeMove(new MakeMoveCommand
            {
                ActionType = request.ActionType,
                PlayerId = this.GetPlayerId()
            });
            return new GameStateViewModel
            {
                Id = request.GameId,
                State = state
            };
        }

        [HttpGet("{gameId:guid}/state")]
        public async Task<GameStateViewModel> GetState(Guid gameId)
        {
            var game = _grainFactory.GetGrain<IGameGrain<RockPaperScissorsSettings>>(gameId);
            var state = await game.GetState();
            return new GameStateViewModel
            {
                Id = gameId,
                State = state
            };
        }
        
        [HttpGet("{gameId:guid}/winners")]
        public async Task<List<Guid>> GetWinners(Guid gameId)
        {
            var game = _grainFactory.GetGrain<IGameGrain<RockPaperScissorsSettings>>(gameId);
            return (await game.GetWinners()).ToList();
        }
        
        [HttpGet("{gameId:guid}/score")]
        public async Task<GameScoreViewModel> GetScore(Guid gameId)
        {
            var game = _grainFactory.GetGrain<IGameGrain<RockPaperScissorsSettings>>(gameId);
            var score = await game.GetScore();
            var playerStats = score.Adapt<IList<PlayerStatsViewModel>>();
            return new GameScoreViewModel()
            {
                PlayerScores = playerStats
            };
        }
    }
}
