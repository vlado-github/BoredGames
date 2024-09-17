using BoredGames.Server.API.Extensions;
using BoredGames.Server.API.Filters;
using BoredGames.Server.Service.Grains.Base;
using BoredGames.Server.Service.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BoredGames.Server.API.Controllers;

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