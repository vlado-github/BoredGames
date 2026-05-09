using BoredGames.Server.GameServer.Extensions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace BoredGames.Server.Tests.Base;

public class TestGameServerFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.SetupOrleans();
        return base.CreateHost(builder);
    }
}