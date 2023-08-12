using BoredGames.Server.Common.Consts;
using Microsoft.AspNetCore.Mvc;

namespace BoredGames.Server.API.Extensions;

public static class ControllerBaseExtensions
{
    public static Guid GetPlayerId(this ControllerBase controller)
    {
        var playerId = controller.Request.Headers[DefaultConsts.PlayerIdHeaderKey];
        if (!string.IsNullOrEmpty(playerId))
        {
            return Guid.Parse(playerId);
        }

        var guid = Guid.NewGuid();
        controller.Response.Headers.Append(DefaultConsts.PlayerIdHeaderKey, guid.ToString());
        return guid;
    }
}