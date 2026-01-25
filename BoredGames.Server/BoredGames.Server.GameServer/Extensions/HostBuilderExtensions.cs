using System.Net;
using BoredGames.Common.Utils;
using BoredGames.Server.GameServer.Mappings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Orleans.Configuration;

namespace BoredGames.Server.GameServer.Extensions;

public static class HostBuilderExtensions
{
    public static IHostBuilder SetupDependencies(this IHostBuilder builder)
    {
        builder.ConfigureServices((hostContext, services) =>
        {
            
        });
        return builder;
    }
    
    public static IHostBuilder SetupOrleans(this IHostBuilder builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }
        builder.RegisterMappings();

        if (CurrentEnvironment.IsLocal())
        {
            builder.UseOrleans((context, siloBuilder) =>
            {
                siloBuilder.UseLocalhostClustering();
            });
        }
        else
        {
            builder.UseOrleans((context, siloBuilder) =>
            {
                var redisHost = context.Configuration["REDIS_HOST"] ?? "redis";

                siloBuilder
                    .UseRedisClustering(options =>
                    {
                        options.ConfigurationOptions = StackExchange.Redis.ConfigurationOptions.Parse(redisHost);
                    })
                    .Configure<ClusterOptions>(options =>
                    {
                        options.ClusterId = "boredgames-cluster";
                        options.ServiceId = "boredgames-gameserver";
                    });
            });
        }

        return builder;
    }
}