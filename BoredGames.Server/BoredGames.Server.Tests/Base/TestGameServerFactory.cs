using Aspire.Hosting.Testing;
using Orleans.TestingHost;

namespace BoredGames.Server.Tests.Base;

public class TestGameServerFactory : IAsyncLifetime
{
    protected InProcessTestCluster Cluster = null!;
    
    public async Task InitializeAsync()
    {
        var builder = new InProcessTestClusterBuilder();
        Cluster = builder.Build();
        await Cluster.DeployAsync();
    }

    public async Task DisposeAsync()
    {
        await Cluster.DisposeAsync();
    }
}
