using BoredGames.Common.Consts;

namespace BoredGames.Common.Configs;

public class ApiKeySettings
{
    public static string HubKeyName => "apiKey";
    public static string ApiKeyName => "X-BORED-GAMES-API-KEY";
    public static string ApiKeyValue => Environment.GetEnvironmentVariable(EnvVarNames.ApiKey);
}