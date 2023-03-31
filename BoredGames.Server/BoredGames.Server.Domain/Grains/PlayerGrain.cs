using BoredGames.Server.Common.Enums;
using Orleans;

namespace BoredGames.Server.Domain.Grains;

public class PlayerGrain : Grain, IPlayerGrain
{
    private int _numberOfWins;
    private int _numberOfLoses;

    public override Task OnActivateAsync(CancellationToken token)
    {
        _numberOfWins = 0;
        _numberOfLoses = 0;
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