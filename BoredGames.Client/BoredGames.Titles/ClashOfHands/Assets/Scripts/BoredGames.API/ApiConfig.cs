using System;

namespace Assets.BoredGames.API
{
    class ApiConfig
    {
        public static Uri BaseUrl = new Uri("https://localhost:7075");
        public static string ApiKey = "b0r3dg4meSScr3am1nGV0iD!";
        public static string HeaderKey = "X-BORED-GAMES-API-KEY";
        public static string HeaderPlayerKey = "X-BORED-GAMES-PLAYER-ID";
        public static string DefaultContentType = "application/json";
        public static string SocketHub = "GameHub";
    }
}
