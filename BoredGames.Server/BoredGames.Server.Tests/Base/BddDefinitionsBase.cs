
using Orleans.TestingHost;
using Xunit.Gherkin.Quick;

namespace BoredGames.Server.Tests.Base;

[FeatureFile("./Base/BddDefinitionsBase.feature")]
public class BddDefinitionsBase : Feature, IAsyncLifetime
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
