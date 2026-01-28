using BoredGames.Common.Consts;
using BoredGames.Common.Utils;
using Serilog;
using Serilog.Enrichers.Sensitive;
using Serilog.Events;

namespace BoredGames.API.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void SetupSerilog(this WebApplicationBuilder? builder)
    {
        if (builder != null)
        {
            if (CurrentEnvironment.IsProduction())
            {
                var logger= new LoggerConfiguration()
                    .WriteTo.Sentry(options =>
                    {
                        options.Dsn = Environment.GetEnvironmentVariable(EnvVarNames.SerilogDsnKey);
                        options.SendDefaultPii = false;
                        options.Debug = true;
                        options.Environment = CurrentEnvironment.Get();
                        options.EnableTracing = true;
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
        }
    }
}