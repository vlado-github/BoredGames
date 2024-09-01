using BoredGames.Server.Domain.Games.Entities;
using BoredGames.Server.Service.Commands;
using BoredGames.Server.Service.Grains.Base;
using BoredGames.Server.Service.ViewModels;
using Mapster;
using Orleans;

namespace BoredGames.Server.Service.Grains;

public class PlayerGrain : Grain, IPlayerGrain
{
    private string? _nickName;
    
    public override Task OnActivateAsync(CancellationToken token)
    {
        return base.OnActivateAsync(token);
    }

    public async Task<GameDefinitionViewModel> CreateGame(CreateGameCommand command)
    {
        var gameId = Guid.NewGuid();
        var gameGrain = GrainFactory.GetGrain<IGameGrain>(gameId);
        gameGrain.Setup(command);

        var playerId = this.GetPrimaryKey();
        var playerNickName = command.PlayerNickName;
        var addPlayerCommand = new AddPlayerCommand
        {
            Id = playerId,
            NickName = playerNickName
        };
        var gameDefinition = await gameGrain.AddPlayerToGame(addPlayerCommand);
        gameDefinition.PlayerId = playerId;
        _nickName = playerNickName;
        return gameDefinition.Adapt<GameDefinitionViewModel>();
    }

    public async Task<GameDefinitionViewModel> JoinGame(JoinGameCommand command)
    {
        var gameGrain = GrainFactory.GetGrain<IGameGrain>(command.GameId);
        _nickName = command.PlayerNickName;
        var addPlayerCommand = new AddPlayerCommand
        {
            Id = this.GetPrimaryKey(),
            NickName = _nickName
        };
        var gameDefinition = await gameGrain.AddPlayerToGame(addPlayerCommand);
        gameDefinition.PlayerId = this.GetPrimaryKey();
        return gameDefinition.Adapt<GameDefinitionViewModel>();
    }

    public Task<PlayerViewModel> GetDetails()
    {
        return Task.FromResult(new PlayerViewModel()
        {
            Id = this.GetPrimaryKey(),
            NickName = _nickName
        });
    }
}