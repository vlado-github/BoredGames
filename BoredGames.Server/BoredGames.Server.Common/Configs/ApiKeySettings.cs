using BoredGames.Server.Common.Consts;
using Microsoft.Extensions.Configuration;

namespace BoredGames.Server.Common.Configs;

public class ApiKeySettings
{
    public static string ApiKeyName => "X-BORED-GAMES-API-KEY";
    public static string ApiKeyValue => Environment.GetEnvironmentVariable(EnvVarNames.ApiKey);
}