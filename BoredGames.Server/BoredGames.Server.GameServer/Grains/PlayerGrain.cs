using BoredGames.Server.GameServer.Commands;
using BoredGames.Server.GameServer.Grains.Base;
using BoredGames.Server.GameServer.ViewModels;
using Mapster;

namespace BoredGames.Server.GameServer.Grains;

public class PlayerGrain : Grain, IPlayerGrain
{
    private string _nickName = string.Empty;
    
    public override Task OnActivateAsync(CancellationToken token)
    {
        return base.OnActivateAsync(token);
    }

    public async Task<GameDefinitionViewModel> CreateGame(CreateGameCommand command)
    {
        var gameId = Guid.NewGuid();
        var gameGrain = GrainFactory.GetGrain<IGameGrain>(gameId);
        await gameGrain.Setup(command);
        
        var gameDefinition = await gameGrain.GetDefinition();
        return gameDefinition.Adapt<GameDefinitionViewModel>();
    }

    public async Task<PlayerViewModel> JoinGame(JoinGameCommand command)
    {
        var gameGrain = GrainFactory.GetGrain<IGameGrain>(command.GameId);
        _nickName = command.PlayerNickName;
        var addPlayerCommand = new AddPlayerCommand
        {
            Id = this.GetPrimaryKey(),
            NickName = _nickName
        };
        await gameGrain.AddPlayerToGame(addPlayerCommand);
        return new PlayerViewModel()
        {
            Id = this.GetPrimaryKey(),
            NickName = _nickName
        };
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