
using Xunit.Gherkin.Quick;

namespace BoredGames.Server.Tests.Base;

[FeatureFile("./Base/BddDefinitionsBase.feature")]
public class BddDefinitionsBase : Feature, IDisposable
{
    protected readonly TestWebApiFactory<API.Program> WebApiInstance;
    protected readonly TestGameServerFactory<GameServer.Program> GameServerInstance;

    protected BddDefinitionsBase()
    {
        GameServerInstance = new TestGameServerFactory<GameServer.Program>();
        WebApiInstance = new TestWebApiFactory<API.Program>();
    }

    public void Dispose()
    {
        GameServerInstance.Dispose();
        WebApiInstance.Dispose();
    }
}