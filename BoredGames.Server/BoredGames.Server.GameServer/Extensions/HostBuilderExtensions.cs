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
                // In order to support multiple hosts forming a cluster, they must listen on different ports.
                // Use the --InstanceId X option to launch subsequent hosts.
                var instanceId = context.Configuration.GetValue<int>("InstanceId");
                var port = 11_111;
                siloBuilder.UseLocalhostClustering(
                    siloPort: port + instanceId,
                    gatewayPort: 30000 + instanceId,
                    primarySiloEndpoint: new IPEndPoint(IPAddress.Loopback, port));
            });
        }
        else
        {
            builder.UseOrleans((context, siloBuilder) =>
            {
                var redisHost = context.Configuration["REDIS_HOST"] ?? "redis";
                var instanceId = int.Parse(context.Configuration["INSTANCE_ID"] ?? "0");

                var siloPort = 11111 + instanceId;
                var gatewayPort = 30000 + instanceId;
                siloBuilder
                    .UseRedisClustering(options =>
                    {
                        options.ConfigurationOptions = StackExchange.Redis.ConfigurationOptions.Parse(redisHost);
                    })
                    .Configure<ClusterOptions>(options =>
                    {
                        options.ClusterId = "boredgames-cluster";
                        options.ServiceId = "boredgames-gameserver";
                    })
                    .ConfigureEndpoints(siloPort: siloPort, gatewayPort: gatewayPort);
            });
        }

        return builder;
    }
}