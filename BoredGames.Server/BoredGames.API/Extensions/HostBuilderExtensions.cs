using System.Net;
using BoredGames.Common.Utils;
using Orleans.Configuration;

namespace BoredGames.API.Extensions;

public static class HostBuilderExtensions
{
    public static void SetupOrleansClient(this IHostBuilder builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.UseOrleansClient(clientBuilder =>
        {
            if (CurrentEnvironment.IsLocal())
            {
                builder.UseOrleansClient(clientBuilder =>
                {
                    clientBuilder.UseLocalhostClustering();
                });
            }
            else
            {
                clientBuilder
                    .UseRedisClustering(options =>
                    {
                        options.ConfigurationOptions = StackExchange.Redis.ConfigurationOptions.Parse("redis");
                    })
                    .Configure<ClusterOptions>(options =>
                    {
                        options.ClusterId = "boredgames-cluster";
                        options.ServiceId = "boredgames-gameserver";
                    });
            }
        });
    }
}