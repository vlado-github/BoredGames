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