using BoredGames.API.Extensions;
using BoredGames.API.Filters;
using BoredGames.API.Models;
using BoredGames.Common.Consts;
using BoredGames.Common.Enums;
using BoredGames.Common.Exceptions;
using BoredGames.Server.Domain.Games.Base;
using BoredGames.Server.GameServer.Commands;
using BoredGames.Server.GameServer.Grains.Base;
using BoredGames.Server.GameServer.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BoredGames.API.Controllers
{
    [ApiKey]
    [Route("api/game")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGrainFactory _grainFactory;
        private readonly string _appBaseUrl;
        
        public GameController(IGrainFactory grainFactory)
        {
            _grainFactory = grainFactory;
            _appBaseUrl = Environment.GetEnvironmentVariable(EnvVarNames.AppBaseUrl);
        }

        [HttpGet("titles")]
        public GameTitlesViewModel GetTitles()
        {
            var result = new GameTitlesViewModel();
            foreach(GameTitle title in Enum.GetValues(typeof(GameTitle)) )
            {
                result.Titles.Add(new GameTitleViewModel()
                {
                    Id = (int) title,
                    Name = title.ToString(),
                    ThumbnailImageUrl = $"{_appBaseUrl}/assets/clashofhands-logo.png",
                    FormSchema = GameFormSchemaFactory.GetInstance(title).ToJson()
                });
            }

            return result;
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
                RequiredNumberOfWins = request.RequiredNumberOfConsecutiveWins,
                NumberOfRounds = request.NumberOfRounds,
                Description = request.Description,
            });
            return gameDefinition;
        }

        [HttpPut("join")]
        public async Task<PlayerViewModel> Join(JoinGame request)
        {
            var playerId = this.GetPlayerId();
            var player = _grainFactory.GetGrain<IPlayerGrain>(playerId);
            var playerDetails = await player.JoinGame(new JoinGameCommand()
            {
                GameId = request.GameId,
                PlayerNickName = request.PlayerNickName
            });
            return playerDetails;
        }

        [HttpPost("makemove")]
        public async Task<GameStateViewModel> MakeMove(MakeMove request)
        {
            var game = _grainFactory.GetGrain<IGameGrain>(request.GameId);
            var gameState = await game.GetState();

            if (gameState.GameStatus != GameStatus.InPlay)
            {
                throw new InvalidActionException("Make move",
                    $"Can't make move since game status is {gameState.GameStatus}.");
            }

            var playerId = this.GetPlayerId();
            var player = _grainFactory.GetGrain<IPlayerGrain>(playerId);
            var playerDetails= await player.GetDetails();
            gameState = await game.MakeMove(new MakeMoveCommand
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

        [HttpGet("{gameId:guid}/definition")]
        public async Task<GameDefinitionViewModel> GetDefinition(Guid gameId)
        {
            var game = _grainFactory.GetGrain<IGameGrain>(gameId);
            return await game.GetDefinition();
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
