using Microsoft.AspNetCore.Mvc;

namespace BoredGames.Server.API.Extensions;

public static class ControllerBaseExtensions
{
    private static readonly string CookieKey = "boredGames.playerId";
    
    public static Guid GetPlayerId(this ControllerBase controller)
    {
        if (controller.Request.Cookies[CookieKey] is { Length: > 0 } idCookie)
        {
            return Guid.Parse(idCookie);
        }

        var guid = Guid.NewGuid();
        controller.Response.Cookies.Append(CookieKey, guid.ToString());
        return guid;
    }
}