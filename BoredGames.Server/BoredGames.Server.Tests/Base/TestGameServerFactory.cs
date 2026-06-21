using Aspire.Hosting.Testing;

namespace BoredGames.Server.Tests.Base;

public class TestGameServerFactory() : DistributedApplicationFactory(typeof(Projects.BoredGames_Server_GameServer))
{

}