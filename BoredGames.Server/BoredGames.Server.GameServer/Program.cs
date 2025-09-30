// See https://aka.ms/new-console-template for more information

using BoredGames.Server.GameServer.Extensions;
using Microsoft.Extensions.Hosting;

using (var host = Host.CreateDefaultBuilder(args).SetupOrleans())
{
    // Start the host
    await host.StartAsync();

    Console.WriteLine("Setup completed.");
    Console.WriteLine("Now you can launch the API.");

    // Exit when any key is pressed
    Console.WriteLine("Press any key to exit.");
    Console.ReadKey();
    await host.StopAsync();

    return 0;
}