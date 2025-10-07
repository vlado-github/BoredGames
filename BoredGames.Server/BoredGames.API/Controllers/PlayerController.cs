using BoredGames.API.Extensions;
using BoredGames.API.Filters;
using BoredGames.Server.GameServer.Grains.Base;
using BoredGames.Server.GameServer.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BoredGames.API.Controllers;

[ApiKey]
[Route("api/player")]
[ApiController]
public class PlayerController : ControllerBase
{
    private readonly IGrainFactory _grainFactory;
        
    public PlayerController(IGrainFactory grainFactory)
    {
        _grainFactory = grainFactory;
    }
    
    [HttpGet("details")]
    public async Task<PlayerViewModel> GetMyDetails()
    {
        var playerId = this.GetPlayerId();
        var player = _grainFactory.GetGrain<IPlayerGrain>(playerId);
        return await player.GetDetails();
    }
}