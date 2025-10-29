using BoredGames.API.Extensions;
using BoredGames.API.Filters;
using BoredGames.API.Models;
using BoredGames.Server.GameServer.Commands;
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
    public async Task<PlayerViewModel> GetProfile()
    {
        var playerId = this.GetPlayerId();
        var player = _grainFactory.GetGrain<IPlayerGrain>(playerId);
        return await player.GetProfile();
    }
    
    [HttpPost]
    public async Task<PlayerViewModel> CreateProfile(CreatePlayerProfile command)
    {
        var playerId = Guid.NewGuid();
        var player = _grainFactory.GetGrain<IPlayerGrain>(playerId);
        return await player.UpdatePlayerProfile(new UpdatePlayerProfileCommand()
        {
            Id = playerId,
            Nickname = command.Nickname
        });
    }
    
    [HttpPut]
    public async Task<PlayerViewModel> AssignNickname(UpdatePlayerProfile command)
    {
        var playerId = this.GetPlayerId();
        var player = _grainFactory.GetGrain<IPlayerGrain>(playerId);
        return await player.UpdatePlayerProfile(new UpdatePlayerProfileCommand()
        {
            Id = playerId,
            Nickname = command.Nickname
        });
    }
}