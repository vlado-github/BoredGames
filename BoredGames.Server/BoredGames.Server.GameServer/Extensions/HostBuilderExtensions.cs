using BoredGames.Common.Utils;
using Microsoft.Extensions.Hosting;
using Orleans.Configuration;
using StackExchange.Redis;

namespace BoredGames.Server.GameServer.Extensions;

public static class HostBuilderExtensions
{
    public static IHostApplicationBuilder SetupDependencies(this IHostApplicationBuilder builder)
    {
        // add services here
        // builder.Services.AddScoped<TService>();
        return builder;
    }
    
    public static IHostApplicationBuilder SetupOrleans(this IHostApplicationBuilder builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (CurrentEnvironment.IsLocal())
        {
            builder.UseOrleans((siloBuilder) =>
            {
                siloBuilder.UseLocalhostClustering();
            });
        }
        else
        {
            builder.AddKeyedRedisClient("redis");
            builder.UseOrleans((siloBuilder) =>
            {
                var redisConnectionString = builder.Configuration["REDIS_HOST"] ?? "redis";
                siloBuilder
                    .UseRedisClustering(options =>
                    {
                        options.ConfigurationOptions = ConfigurationOptions.Parse(redisConnectionString!);
                    })
                    .Configure<ClusterOptions>(options =>
                    {
                        options.ClusterId = "boredgames-cluster";
                        options.ServiceId = "boredgames-gameserver";
                    });
                siloBuilder
                    .AddRedisGrainStorageAsDefault(options =>
                    {
                        options.ConfigurationOptions = ConfigurationOptions.Parse(redisConnectionString!);
                    });
            });
        }

        return builder;
    }
}