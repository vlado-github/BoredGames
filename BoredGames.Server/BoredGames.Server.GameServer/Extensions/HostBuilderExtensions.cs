using System;
using BoredGames.Common.Consts;
using BoredGames.Common.Utils;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.Configuration;
using Orleans.Hosting;
using Sentry;
using Serilog;
using Serilog.Enrichers.Sensitive;
using Serilog.Events;
using StackExchange.Redis;

namespace BoredGames.Server.GameServer.Extensions;

public static class HostBuilderExtensions
{
    public static IHostApplicationBuilder SetupDependencies(this IHostApplicationBuilder builder)
    {
        // add services here
        // builder.Services.AddScoped<TService>();
        
        if (CurrentEnvironment.IsProduction())
        {
            var logger= new LoggerConfiguration()
                .WriteTo.Sentry(options =>
                {
                    options.Dsn = Environment.GetEnvironmentVariable(EnvVarNames.SerilogDsnKey);
                    options.SendDefaultPii = false;
                    options.Debug = true;
                    options.Environment = CurrentEnvironment.Get();
                    options.EnableMetrics = true;
                    options.TracesSampleRate = 1.0;
                    options.AttachStacktrace = true;
                    options.DiagnosticLevel = SentryLevel.Error;
                    options.MinimumBreadcrumbLevel = LogEventLevel.Debug;
                    options.MinimumEventLevel = LogEventLevel.Warning;
                })
                .WriteTo.Console()
                .Enrich.FromLogContext()
                .Enrich.WithSensitiveDataMasking(new SensitiveDataEnricherOptions())
                .Enrich.WithCorrelationId()
                .CreateLogger();
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);
        }
        else
        {
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithSensitiveDataMasking(new SensitiveDataEnricherOptions())
                .Enrich.WithCorrelationId()
                .CreateLogger();
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);
        }
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
                siloBuilder
                    .AddMemoryGrainStorage("Default")
                    .UseLocalhostClustering();
            });
        }
        else
        {
            builder.AddKeyedRedisClient("redis");
            builder.UseOrleans((siloBuilder) =>
            {
                var redisConnectionString = builder.Configuration["REDIS_HOST"] ?? "redis";
                siloBuilder
                    .AddRedisGrainStorage("Default", options =>
                    { 
                        var configuration = ConfigurationOptions.Parse(redisConnectionString!);
                        options.ConfigurationOptions = configuration;
                    })
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