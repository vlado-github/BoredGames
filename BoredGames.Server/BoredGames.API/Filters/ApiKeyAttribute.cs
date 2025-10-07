using BoredGames.Common.Configs;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BoredGames.API.Filters;

public class ApiKeyAttribute :  ActionFilterAttribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        context.HttpContext.Request.Headers.TryGetValue(ApiKeySettings.ApiKeyName, out var apiKeyValue);
        if (apiKeyValue != ApiKeySettings.ApiKeyValue)
        {
            throw new UnauthorizedAccessException("ApiKey is not correct.");
        }
    }
}