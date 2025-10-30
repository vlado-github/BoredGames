using BoredGames.Common.Configs;
using Microsoft.AspNetCore.SignalR;

namespace BoredGames.API.Filters;

public class HubKeyAttribute : IHubFilter
{
    public async ValueTask<object?> InvokeMethodAsync(
        HubInvocationContext invocationContext, 
        Func<HubInvocationContext, ValueTask<object?>> next)
    {
        var httpContext = invocationContext.Context.GetHttpContext();
        if (httpContext == null)
        {
            throw new UnauthorizedAccessException("HttpContext is empty.");
        }

        httpContext.Request.Query.TryGetValue(ApiKeySettings.HubKeyName, out var apiKeyValue);
        if (apiKeyValue != ApiKeySettings.ApiKeyValue)
        {
            throw new UnauthorizedAccessException("ApiKey is not correct.");
        }
        var result = await next(invocationContext);

        return result;
    }
}