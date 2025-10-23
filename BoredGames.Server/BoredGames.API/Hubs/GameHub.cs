using BoredGames.API.Models;
using BoredGames.Common.Enums;
using BoredGames.Common.Exceptions;
using BoredGames.Server.GameServer.Commands;
using BoredGames.Server.GameServer.Grains.Base;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace BoredGames.API.Hubs;

public class GameHub : Hub
{
    private readonly IGrainFactory _grainFactory;
    private readonly ILogger<GameHub> _logger;
    
    public GameHub(IGrainFactory grainFactory, ILogger<GameHub> logger)
    {
        _grainFactory = grainFactory;
        _logger = logger;
    }
    
    public async Task JoinGameGroup(string gameId, string playerNickname)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
        await Clients.Group(gameId).SendAsync("OnPlayerJoined", playerNickname);
        _logger.LogInformation($"Player {playerNickname} joined the game {gameId}");
        
        var parsed = Guid.TryParse(gameId, out var gameIdAsGuid);
        if (!parsed)
        {
            _logger.LogError($"Invalid gameId. Can't parse gameId {gameId}");
        }
        var game = _grainFactory.GetGrain<IGameGrain>(gameIdAsGuid);
        var gameState = await game.GetState();
        await Clients
            .Group(gameId)
            .SendAsync("OnGameStateReceived", JsonConvert.SerializeObject(gameState));
    }
    
    public async Task MakeMove(MakeMove makeMoveAction)
    {
        var parsed = Guid.TryParse(makeMoveAction.GameId, out var gameIdAsGuid);
        if (!parsed)
        {
            _logger.LogError($"Invalid gameId. Can't parse gameId {makeMoveAction.GameId}");
        }
        parsed = Guid.TryParse(makeMoveAction.PlayerId, out var playerIdAsGuid);
        if (!parsed)
        {
            _logger.LogError($"Invalid playerId. Can't parse playerId {makeMoveAction.PlayerId}");
        }
        
        var game = _grainFactory.GetGrain<IGameGrain>(gameIdAsGuid);
        var gameState = await game.GetState();

        if (gameState.GameStatus != GameStatus.InPlay)
        {
            throw new InvalidActionException("Make move",
                $"Can't make move since game status is {gameState.GameStatus}.");
        }

        var player = _grainFactory.GetGrain<IPlayerGrain>(playerIdAsGuid);
        var playerDetails= await player.GetProfile();
        gameState = await game.MakeMove(new MakeMoveCommand
        {
            ActionType = makeMoveAction.ActionType,
            PlayerId = playerIdAsGuid,
            PlayerNickName = playerDetails.NickName
        });
            
        await Clients
            .Group(gameState.GameId.ToString())
            .SendAsync("OnGameStateReceived", gameState);
    }
}