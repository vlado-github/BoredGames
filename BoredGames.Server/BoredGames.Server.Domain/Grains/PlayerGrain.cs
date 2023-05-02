using BoredGames.Server.Domain.Commands;
using BoredGames.Server.Domain.Games.Entities;
using BoredGames.Server.Domain.Games.RockPaperScissors;
using BoredGames.Server.Domain.Grains.Base;
using Orleans;

namespace BoredGames.Server.Domain.Grains;

public class PlayerGrain : Grain, IPlayerGrain
{
    public override Task OnActivateAsync(CancellationToken token)
    {
        return base.OnActivateAsync(token);
    }

    public async Task<GameDefinition> CreateGame(CreateGameCommand command)
    {
        var gameId = Guid.NewGuid();
        var gameGrain = GrainFactory.GetGrain<IGameGrain<RockPaperScissorsSettings>>(gameId);
        gameGrain.Setup(command);
        
        var playerId = this.GetPrimaryKey();
        var gameDefinition = await gameGrain.AddPlayerToGame(playerId);
        return gameDefinition;
    }

    public async Task<GameDefinition> JoinGame(JoinGameCommand command)
    {
        var gameGrain = GrainFactory.GetGrain<IGameGrain<RockPaperScissorsSettings>>(command.GameId);
        var gameDefinition = await gameGrain.AddPlayerToGame(this.GetPrimaryKey());
        return gameDefinition;
    }
}