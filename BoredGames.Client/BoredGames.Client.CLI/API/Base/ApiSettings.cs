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
    public string HeaderName => "X-BORED-GAMES-API-KEY";
    public string HeaderValue => Environment.GetEnvironmentVariable("BORED_GAMES_API_KEY");
}