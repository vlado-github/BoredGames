using BoredGames.Server.API.Extensions;
using BoredGames.Server.API.Filters;
using BoredGames.Server.API.Models;
using BoredGames.Server.Common.Enums;
using BoredGames.Server.Domain.Games.Entities;
using BoredGames.Server.Service.Commands;
using BoredGames.Server.Service.Grains.Base;
using BoredGames.Server.Service.ViewModels;
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

        [HttpGet("titles")]
        public Task<IList<GameTitleViewModel>> GetTitles()
        {
            var result = new List<GameTitleViewModel>();
            foreach(GameTitle title in Enum.GetValues(typeof(GameTitle)) )
            {
                result.Add(new GameTitleViewModel()
                {
                    Id = (int) title,
                    Name = title.ToString()
                });
            }

            return Task.FromResult<IList<GameTitleViewModel>>(result);
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
                Description = request.Description,
                PlayerNickName = request.PlayerNickName
            });
            return gameDefinition;
        }

        [HttpPut("join")]
        public async Task<GameDefinitionViewModel> Join(JoinGame request)
        {
            var playerId = this.GetPlayerId();
            var player = _grainFactory.GetGrain<IPlayerGrain>(playerId);
            var gameDefinition = await player.JoinGame(new JoinGameCommand()
            {
                GameId = request.GameId,
                PlayerNickName = request.PlayerNickName
            });
            return gameDefinition;
        }

        [HttpPost("makemove")]
        public async Task<GameStateViewModel> MakeMove(MakeMove request)
        {
            var game = _grainFactory.GetGrain<IGameGrain>(request.GameId);

            var playerId = this.GetPlayerId();
            var player = _grainFactory.GetGrain<IPlayerGrain>(playerId);
            var playerDetails= await player.GetDetails();
            var gameState = await game.MakeMove(new MakeMoveCommand
            {
                ActionType = request.ActionType,
                PlayerId = playerId,
                PlayerNickName = playerDetails.NickName
            });
            return gameState;
        }

        [HttpGet("{gameId:guid}/state")]
        public async Task<GameStateViewModel> GetState(Guid gameId)
        {
            var game = _grainFactory.GetGrain<IGameGrain>(gameId);
            return await game.GetState();
        }
        
        [HttpGet("{gameId:guid}/winners")]
        public async Task<IList<PlayerViewModel>> GetWinners(Guid gameId)
        {
            var game = _grainFactory.GetGrain<IGameGrain>(gameId);
            return await game.GetWinners();
        }
        
        [HttpGet("{gameId:guid}/score")]
        public async Task<GameScoreViewModel> GetScore(Guid gameId)
        {
            var game = _grainFactory.GetGrain<IGameGrain>(gameId);
            return await game.GetScore();
        }
    }
}
