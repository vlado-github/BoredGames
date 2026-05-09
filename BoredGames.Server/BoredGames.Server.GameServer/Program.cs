// See https://aka.ms/new-console-template for more information

using BoredGames.Server.GameServer.Extensions;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args)
    .SetupDependencies()
    .SetupOrleans();
using (var host = builder.Build())
{
    // Start the host
    await host.RunAsync();
}

// In order to enable tests to run a test instance of a host
namespace BoredGames.Server.GameServer
{
    public partial class Program { }
}