using BoredGames.Common.Enums;
using BoredGames.Server.GameServer.Commands;
using BoredGames.Server.GameServer.Grains.Base;
using Orleans.TestingHost;

namespace BoredGames.Server.Tests.UnitTests;

public class CreateGameGrainTests : IAsyncLifetime
{
    private InProcessTestCluster _cluster = null!;
    
    public async Task InitializeAsync()
    {
        var builder = new InProcessTestClusterBuilder();
        _cluster = builder.Build();
        await _cluster.DeployAsync();
    }

    public async Task DisposeAsync()
    {
        await _cluster.DisposeAsync();
    }

    [Fact]
    public async Task CreateGame()
    {
        var playerGrain = _cluster.Client.GetGrain<IPlayerGrain>(Guid.NewGuid());
        var result = await playerGrain.CreateGame(new CreateGameCommand()
        {
            Title = GameTitle.ClashOfHands,
            NumberOfPlayers = 2,
            NumberOfRounds = 1,
            RequiredNumberOfWins = 1
        });
        Assert.NotNull(result);
        Assert.True(result.GameId != Guid.Empty);
    }
}