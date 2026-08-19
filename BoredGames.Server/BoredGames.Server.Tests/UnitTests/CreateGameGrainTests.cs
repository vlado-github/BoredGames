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
        builder.ConfigureSilo((options, siloBuilder) =>
            siloBuilder.AddMemoryGrainStorage("Default"));
        _cluster = builder.Build();
        await _cluster.DeployAsync();
    }

    public async Task DisposeAsync()
    {
        await _cluster.DisposeAsync();
    }

    [Theory]
    [InlineData(GameTitle.ClashOfHands)]
    [InlineData(GameTitle.TicTacToe)]
    public async Task CreateGame(GameTitle gameTitle)
    {
        // Arrange
        var playerGrain = _cluster.Client.GetGrain<IPlayerGrain>(Guid.NewGuid());
        
        // Act
        var result = await playerGrain.CreateGame(new CreateGameCommand()
        {
            Title = gameTitle,
            NumberOfPlayers = 2,
            NumberOfRounds = 1,
            RequiredNumberOfWins = 1
        });
        
        // Assert
        Assert.NotNull(result);
        Assert.True(result.GameId != Guid.Empty);
    }
}