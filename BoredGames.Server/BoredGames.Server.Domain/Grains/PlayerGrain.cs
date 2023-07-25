using Bogus;
using BoredGames.Server.Domain.Commands;
using BoredGames.Server.Domain.Games.Base;
using BoredGames.Server.Domain.Games.Entities;
using BoredGames.Server.Domain.Games.RockPaperScissors;
using BoredGames.Server.Domain.Grains.Base;
using Orleans;

namespace BoredGames.Server.Domain.Grains;

public class PlayerGrain : Grain, IPlayerGrain
{
    private Player _player;
    
    public override Task OnActivateAsync(CancellationToken token)
    {
        _player = new Player(this.GetPrimaryKey());
        return base.OnActivateAsync(token);
    }

    public async Task<GameDefinition> CreateGame(CreateGameCommand command)
    {
        var gameId = Guid.NewGuid();
        var gameGrain = GrainFactory.GetGrain<IGameGrain>(gameId);
        gameGrain.Setup(command);

        var playerId = this.GetPrimaryKey();
        var playerNickName = command.PlayerNickName ?? _player.NickName;
        _player = new Player(playerId, playerNickName);
        var gameDefinition = await gameGrain.AddPlayerToGame(_player);
        return gameDefinition;
    }

    public async Task<GameDefinition> JoinGame(JoinGameCommand command)
    {
        var gameGrain = GrainFactory.GetGrain<IGameGrain>(command.GameId);
        var playerNickName = command.PlayerNickName ?? _player.NickName;
        _player = new Player(this.GetPrimaryKey(), playerNickName);
        var gameDefinition = await gameGrain.AddPlayerToGame(_player);
        return gameDefinition;
    }

    public async Task<Player> GetPlayerDetails()
    {
        return await Task.FromResult(_player);
    }
}