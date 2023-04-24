using BoredGames.Client.CLI.API.Base;
using BoredGames.Client.CLI.Runtime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BoredGames.Client.CLI;

class Program
{
    static async Task Main(string[] args)
    {
        Console.CancelKeyPress += delegate(object? sender, ConsoleCancelEventArgs e) 
        {
            e.Cancel = true;
        };
        
        var services = new ServiceCollection();
        ConfigureServices(services);
        var serviceProvider = services.BuildServiceProvider();
        
        var executor = serviceProvider.GetRequiredService<IExecutor>();
        await executor.Execute();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        var environment = Environment.GetEnvironmentVariable("DOTNET_ENV");
        var config = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.{environment}.json")
            .Build();
        var apiSettings = new ApiSettings(config.GetSection(nameof(ApiSettings)));

        services.AddSingleton<IConfigurationRoot>(cr => config);
        services.AddSingleton<ApiSettings>(settings => apiSettings);
        services.AddBoredGamesApi(apiSettings);
        services.AddScoped<IExecutor, Executor>();
    }

    
}