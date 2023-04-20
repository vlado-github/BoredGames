using Microsoft.Extensions.Configuration;

namespace BoredGames.Client.CLI.API.Base;

public class ApiSettings
{
    private readonly IConfigurationSection _configSection;
    
    public ApiSettings(IConfigurationSection configSection)
    {
        _configSection = configSection;
    }

    public string BaseUrl => _configSection[nameof(BaseUrl)];
    public string HeaderApiKeyName => "X-BORED-GAMES-API-KEY";
    public string HeaderApiKeyValue => Environment.GetEnvironmentVariable("BORED_GAMES_API_KEY");
    public string HeaderPlayerIdName => "boredgames.playerid";
    public string HeaderPlayerIdValue => Guid.NewGuid().ToString();
}