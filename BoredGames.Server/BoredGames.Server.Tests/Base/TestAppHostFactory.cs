using Aspire.Hosting.Testing;
using Microsoft.Extensions.Hosting;

namespace BoredGames.Server.Tests.Base;

public class TestAppHostFactory() : DistributedApplicationFactory(typeof(Projects.BoredGames_Aspire_AppHost))
{
}