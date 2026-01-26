using System.Net.Http.Headers;
using System.Text;
using BoredGames.API.Models;
using Keycloak.AuthServices.Common;
using Microsoft.AspNetCore.Mvc;

namespace BoredGames.API.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly KeycloakInstallationOptions? _keycloakOptions;
    
    public AuthController(IConfiguration configuration)
    {
        _keycloakOptions = configuration.GetKeycloakOptions<KeycloakInstallationOptions>();
    }
    
    [HttpGet("token")]
    public async Task<AuthToken> GetToken()
    {
        if (_keycloakOptions == null)
        {
            throw new InvalidOperationException("KeycloakOptions not configured");
        }
        
        using (var client = new HttpClient())
        {
            var clientCredentials = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{_keycloakOptions.Resource}:{_keycloakOptions.Credentials.Secret}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", clientCredentials);
            
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
            });

            var result =
                await client.PostAsync(
                    $"{_keycloakOptions.AuthServerUrl}/realms/boredgames/protocol/openid-connect/token", 
                    formContent);

            if (result.IsSuccessStatusCode)
            {
                var authToken = result.Content.ReadFromJsonAsync<AuthToken>().Result;
                if (authToken == null || string.IsNullOrEmpty(authToken.AccessToken))
                {
                    throw new Exception("Invalid token");
                }
                return authToken;
            }
            
            throw new HttpProtocolException((int)result.StatusCode, result.ReasonPhrase, null);
        }
    }
}