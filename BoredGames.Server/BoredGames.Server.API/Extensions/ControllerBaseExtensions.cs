using Microsoft.AspNetCore.Mvc;

namespace BoredGames.Server.API.Extensions;

public static class ControllerBaseExtensions
{
    private static readonly string HeaderKey = "boredgames.playerid";
    
    public static Guid GetPlayerId(this ControllerBase controller)
    {
        var playerId = controller.Request.Headers[HeaderKey];
        if (!string.IsNullOrEmpty(playerId))
        {
            return Guid.Parse(playerId);
        }

        var guid = Guid.NewGuid();
        controller.Response.Headers.Append(HeaderKey, guid.ToString());
        return guid;
    }
}