using System.Diagnostics;
using BoredGames.Common.Consts;
using Microsoft.AspNetCore.Mvc;

namespace BoredGames.API.Extensions;

public static class ControllerBaseExtensions
{
    public static Guid GetPlayerId(this ControllerBase controller)
    {
        var playerId = controller.Request.Headers[DefaultConsts.PlayerIdHeaderKey];
        if (string.IsNullOrEmpty(playerId.ToString()))
        {
            throw new ApplicationException("No player ID was provided in request header.");
        }
        
        return Guid.Parse(playerId.ToString());
    }
}