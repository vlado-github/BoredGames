using System.Net;
using BoredGames.Client.CLI.API;
using BoredGames.Client.CLI.API.Base;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Refit;

namespace BoredGames.Client.CLI;

public static class DependencyInjectionRegistry
{
    public static readonly HttpStatusCode[] httpStatusCodesWorthRetrying =
    {
        HttpStatusCode.RequestTimeout, //408
        HttpStatusCode.BadGateway, //502
        HttpStatusCode.ServiceUnavailable, //503
        HttpStatusCode.GatewayTimeout, //504
    };
    
    public static void AddBoredGamesApi(this IServiceCollection services, ApiSettings apiSettings)
    {
        services.AddTransient<ApiKeyHeaderHandler>();

        var unavailablePolicy = Policy
            .Handle<HttpRequestException>()
            .OrResult<HttpResponseMessage>(r => 
                httpStatusCodesWorthRetrying.Contains(r.StatusCode))
            .WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(5), 
                TimeSpan.FromSeconds(10), 
                TimeSpan.FromSeconds(30), 
            });

        services.AddRefitClient<IBoredGamesApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiSettings.BaseUrl))
            .AddHttpMessageHandler<ApiKeyHeaderHandler>()
            .AddPolicyHandler(unavailablePolicy);
    }
}