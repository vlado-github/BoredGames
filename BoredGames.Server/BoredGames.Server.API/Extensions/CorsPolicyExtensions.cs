using BoredGames.Server.Common.Consts;
using BoredGames.Server.Common.Utils;

namespace BoredGames.Server.API.Extensions;

public static class CorsPolicyExtensions
{
    public static string CorsPolicyName = "BoredGamesOrigins";

    public static void AddCustomCors(this IServiceCollection services)
    {
        //CORS
        services.AddCors(options =>
        {
            if (!CurrentEnvironment.IsLocal())
            {
                options.AddPolicy(name: CorsPolicyName,
                    policy =>
                    {
                        var corsOriginUrlString = Environment.GetEnvironmentVariable(EnvVarNames.CorsOrigin);
                        if (string.IsNullOrEmpty(corsOriginUrlString))
                        {
                            throw new ArgumentException(
                                "CORS origin URL can't be empty. " +
                                "Check if Environment Variable has been set.");
                        }

                        policy.WithOrigins(corsOriginUrlString)
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            }
            else
            {
                options.AddPolicy(name: CorsPolicyName,
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            }
        });
    }
}