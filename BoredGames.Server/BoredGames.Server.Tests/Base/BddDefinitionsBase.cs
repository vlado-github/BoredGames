
using System.Threading.Tasks;
using Orleans.Hosting;
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
