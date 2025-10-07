using BoredGames.API;
using Orleans;
using Xunit.Gherkin.Quick;

namespace BoredGames.Server.Tests.Base;

[FeatureFile("./Base/BddDefinitionsBase.feature")]
public class BddDefinitionsBase : Feature, IDisposable
{
    protected readonly TestWebApplicationFactory<Program> Application;

    protected BddDefinitionsBase()
    {
        Application = new TestWebApplicationFactory<Program>();
    }

    public void Dispose()
    {
        Application.Dispose();
    }
}