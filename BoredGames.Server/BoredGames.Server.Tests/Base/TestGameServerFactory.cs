using Aspire.Hosting.Testing;
using Orleans.TestingHost;

namespace BoredGames.Server.Tests.Base;

public class TestGameServerFactory : IAsyncLifetime
{
    protected InProcessTestCluster Cluster = null!;
    
    public async Task InitializeAsync()
    {
        var builder = new InProcessTestClusterBuilder();
        builder.ConfigureSilo((options, siloBuilder) =>
            siloBuilder.AddMemoryGrainStorage("Default"));
        Cluster = builder.Build();
        await Cluster.DeployAsync();
    }

    public async Task DisposeAsync()
    {
        await Cluster.DisposeAsync();
    }
}
