using BoredGames.Server.Common.Enums;
using BoredGames.Server.Domain.Grains.Base;
using Orleans;

namespace BoredGames.Server.Domain.Grains;

public class PlayerGrain : Grain, IPlayerGrain
{
    public override Task OnActivateAsync(CancellationToken token)
    {
        return base.OnActivateAsync(token);
    }

    public async Task<Guid> CreateGame()
    {
        var gameId = Guid.NewGuid();
        var gameGrain = GrainFactory.GetGrain<IGameGrain>(gameId);

        var playerId = this.GetPrimaryKey();
        await gameGrain.AddPlayerToGame(playerId);
        return gameId;
    }

    public async Task<GameState> JoinGame(Guid gameId)
    {
        var gameGrain = GrainFactory.GetGrain<IGameGrain>(gameId);
        var state = await gameGrain.AddPlayerToGame(this.GetPrimaryKey());
        return state;
    }
}