using System;
using BoredGames.Common.Utils;
using Microsoft.Extensions.Hosting;
using Orleans.Configuration;
using Orleans.Hosting;
using StackExchange.Redis;

namespace BoredGames.API.Extensions;

public static class HostBuilderExtensions
{
    public static void SetupOrleansClient(this IHostApplicationBuilder builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (CurrentEnvironment.IsLocal())
        {
            builder.UseOrleansClient(clientBuilder =>
            {
                builder.UseOrleansClient(options => { options.UseLocalhostClustering(); });
            });
        }
        else
        {
            builder.AddKeyedRedisClient("redis");
            builder.UseOrleansClient(clientBuilder =>
            {
                var redisConnectionString = builder.Configuration["REDIS_HOST"] ?? "redis";
                clientBuilder
                    .UseRedisClustering(options =>
                    {
                        options.ConfigurationOptions = ConfigurationOptions.Parse(redisConnectionString!);
                    })
                    .Configure<ClusterOptions>(options =>
                    {
                        options.ClusterId = "boredgames-cluster";
                        options.ServiceId = "boredgames-gameserver";
                    });
            });
        }
    }
}