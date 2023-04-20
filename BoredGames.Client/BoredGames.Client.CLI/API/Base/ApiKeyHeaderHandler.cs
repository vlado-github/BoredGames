using System.Net.Http.Headers;

namespace BoredGames.Client.CLI.API.Base;

public class ApiKeyHeaderHandler : DelegatingHandler
{
    private readonly ApiSettings _settings;
    
    public ApiKeyHeaderHandler(ApiSettings settings)
    {
        _settings = settings;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        request.Headers.Authorization = 
            new AuthenticationHeaderValue(_settings.HeaderApiKeyName, _settings.HeaderApiKeyValue);
        request.Headers.Add(_settings.HeaderPlayerIdName, _settings.HeaderPlayerIdValue);
        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}