using BoredGames.Common.Consts;
using BoredGames.Common.Utils;

namespace BoredGames.API.Extensions;

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
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            }
            else
            {
                options.AddPolicy(name: CorsPolicyName,
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:5173")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
            }
        });
    }
}