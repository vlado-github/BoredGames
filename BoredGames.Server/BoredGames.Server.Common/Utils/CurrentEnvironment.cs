using BoredGames.Server.Common.Consts;

namespace BoredGames.Server.Common.Utils;

public static class CurrentEnvironment
{
    public static string AspNetCoreEnvVar = "ASPNETCORE_ENVIRONMENT";
    
    public static bool IsLocal()
    {
        var env = Environment.GetEnvironmentVariable(AspNetCoreEnvVar);
        return env == EnvironmentConsts.Local || string.IsNullOrEmpty(env);
    }

    public static bool IsDevelopment()
    {
        var env = Environment.GetEnvironmentVariable(AspNetCoreEnvVar);
        return env == EnvironmentConsts.Development;
    }

    public static bool IsStaging()
    {
        var env = Environment.GetEnvironmentVariable(AspNetCoreEnvVar);
        return env == EnvironmentConsts.Staging;
    }

    public static bool IsProduction()
    {
        var env = Environment.GetEnvironmentVariable(AspNetCoreEnvVar);
        return env == EnvironmentConsts.Production;
    }
}