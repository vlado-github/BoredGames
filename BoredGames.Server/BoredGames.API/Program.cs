using BoredGames.API.Extensions;
using BoredGames.API.Filters;
using BoredGames.API.Hubs;
using BoredGames.API.Middlewares;
using BoredGames.API.Models;
using BoredGames.Common.Utils;
using FluentValidation;
using FluentValidation.AspNetCore;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);
builder.Host.SetupOrleansClient();
builder.SetupSerilog();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(MakeMoveValidator));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCustomCors();
builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.AddFilter<HubKeyAttribute>();
});

// Setup Auth
builder.Services
    .AddKeycloakWebApiAuthentication(builder.Configuration, options =>
    {
        builder.Configuration.GetSection("Authentication:Schemes:Bearer").Bind(options);
    });
builder.Services
    .AddAuthorization()
    .AddKeycloakAuthorization(builder.Configuration);


var app = builder.Build();

// Swagger
if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

// CORS
app.UseCors(CorsPolicyExtensions.CorsPolicyName);

var webSocketOptions = new WebSocketOptions();
foreach (var originUrl in CorsPolicyExtensions.GetCorsOriginURLs())
{
    webSocketOptions.AllowedOrigins.Add($"https://{originUrl}");
}
app.UseWebSockets(webSocketOptions);

// Default Middlewares
if (CurrentEnvironment.IsLocal())
{
    app.UseHttpsRedirection();
}
app.UseAuthentication();
app.UseAuthorization();

// Custom Middlewares
app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

// Controllers and Hubs
app.MapControllers();
app.MapHub<GameHub>($"/hubs/{nameof(GameHub)}");
app.Run();

// In order to enable tests to run a test instance of a host
namespace BoredGames.API
{
    public partial class Program { }
}