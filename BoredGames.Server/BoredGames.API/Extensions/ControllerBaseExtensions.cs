using BoredGames.Common.Consts;
using Microsoft.AspNetCore.Mvc;

namespace BoredGames.API.Extensions;

public static class ControllerBaseExtensions
{
    public static Guid GetPlayerId(this ControllerBase controller)
    {
        var playerId = controller.Request.Headers[DefaultConsts.PlayerIdHeaderKey];
        if (string.IsNullOrEmpty(playerId))
        {
            return Guid.NewGuid();
        }

        return Guid.Parse(playerId);
    }
}