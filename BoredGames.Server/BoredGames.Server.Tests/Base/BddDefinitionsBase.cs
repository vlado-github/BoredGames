
using Xunit.Gherkin.Quick;

namespace BoredGames.Server.Tests.Base;

[FeatureFile("./Base/BddDefinitionsBase.feature")]
public class BddDefinitionsBase : Feature, IDisposable
{
    protected readonly TestAppHostFactory AppHostInstance;
    protected readonly TestGameServerFactory GameServerInstance;

    protected BddDefinitionsBase()
    {
        GameServerInstance = new TestGameServerFactory();
        AppHostInstance = new TestAppHostFactory();
    }

    public void Dispose()
    {
        GameServerInstance.Dispose();
        AppHostInstance.Dispose();
    }
}