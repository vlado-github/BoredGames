using BoredGames.Server.API.Extensions;
using BoredGames.Server.API.Models;
using BoredGames.Server.Domain.Grains.Base;
using Microsoft.AspNetCore.Mvc;

namespace BoredGames.Server.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGrainFactory _grainFactory;
        
        public GameController(IGrainFactory grainFactory)
        {
            _grainFactory = grainFactory;
        }
        
        [HttpPost]
        public async Task<Guid> CreateGame()
        {
            var playerId = this.GetPlayerId();
            var player = _grainFactory.GetGrain<IPlayerGrain>(playerId);
            var gameId = await player.CreateGame();
            return gameId;
        }

        [HttpPut]
        public async Task<GameViewModel> Join(Guid gameId)
        {
            var playerId = this.GetPlayerId();
            var player = _grainFactory.GetGrain<IPlayerGrain>(playerId);
            var state = await player.JoinGame(gameId);
            return new GameViewModel
            {
                Id = gameId,
                State = state
            };
        }
    }
}
