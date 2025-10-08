using System;
using System.Configuration;

namespace Assets.BoredGames.API
{
    class ApiConfig
    {
        public Uri BaseUrl { get; private set; }
        public string ApiKey { get; private set; }
        public string HeaderKey { get; private set; }

        public ApiConfig()
        {
            BaseUrl = new Uri("https://localhost:7075");
            ApiKey = "b0r3dg4meSScr3am1nGV0iD!";
            HeaderKey = "X-BORED-GAMES-API-KEY";
        }

        public ApiConfig(Uri baseUrl, string apiKey, string headerKey)
        {
            BaseUrl = baseUrl;
            ApiKey = apiKey;
            HeaderKey = headerKey;
        }

        
    }
}
