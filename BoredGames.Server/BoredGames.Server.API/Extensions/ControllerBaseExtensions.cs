using Microsoft.AspNetCore.Mvc;

namespace BoredGames.Server.API.Extensions;

public static class ControllerBaseExtensions
{
    private static readonly string PlayerIdHeaderKey = "X-BORED-GAMES-PLAYER-ID";
    
    public static Guid GetPlayerId(this ControllerBase controller)
    {
        var playerId = controller.Request.Headers[PlayerIdHeaderKey];
        if (!string.IsNullOrEmpty(playerId))
        {
            return Guid.Parse(playerId);
        }

        var guid = Guid.NewGuid();
        controller.Response.Headers.Append(PlayerIdHeaderKey, guid.ToString());
        return guid;
    }
}