using BoredGames.Server.API.Extensions;
using BoredGames.Server.API.Filters;
using BoredGames.Server.API.Models;
using BoredGames.Server.API.Views;
using BoredGames.Server.Domain.Commands;
using BoredGames.Server.Domain.Grains.Base;
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
        public async Task<Guid> Create(CreateGame request)
        {
            var playerId = this.GetPlayerId();
            var player = _grainFactory.GetGrain<IPlayerGrain>(playerId);
            var gameId = await player.CreateGame(new CreateGameCommand
            {
                NumberOfPlayers = request.NumberOfPlayers,
                NumberOfWins = request.NumberOfWins
            });
            return gameId;
        }

        [HttpPut("join")]
        public async Task<GameViewModel> Join(JoinGame request)
        {
            var playerId = this.GetPlayerId();
            var player = _grainFactory.GetGrain<IPlayerGrain>(playerId);
            var state = await player.JoinGame(request.GameId);
            return new GameViewModel
            {
                Id = request.GameId,
                State = state
            };
        }

        [HttpPost("makemove")]
        public async Task<GameViewModel> MakeMove(MakeMove request)
        {
            var game = _grainFactory.GetGrain<IGameGrain>(request.GameId);
            var state = await game.MakeMove(new MakeMoveCommand
            {
                ActionType = request.ActionType,
                PlayerId = this.GetPlayerId()
            });
            return new GameViewModel
            {
                Id = request.GameId,
                State = state
            };
        }

        [HttpGet("{gameId:guid}/state")]
        public async Task<GameViewModel> GetState(Guid gameId)
        {
            var game = _grainFactory.GetGrain<IGameGrain>(gameId);
            var state = await game.GetState();
            return new GameViewModel
            {
                Id = gameId,
                State = state
            };
        }
        
        [HttpGet("{gameId:guid}/winners")]
        public async Task<List<Guid>> GetWinners(Guid gameId)
        {
            var game = _grainFactory.GetGrain<IGameGrain>(gameId);
            return (await game.GetWinners()).ToList();
        }
    }
}
