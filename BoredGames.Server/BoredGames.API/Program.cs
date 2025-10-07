using BoredGames.API.Extensions;
using BoredGames.API.Middlewares;
using BoredGames.API.Models;
using BoredGames.Common.Utils;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Host.SetupOrleansClient();
builder.SetupSerilog();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(MakeMoveValidator));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCustomCors();


var app = builder.Build();
if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}
app.UseCors(CorsPolicyExtensions.CorsPolicyName);
if (CurrentEnvironment.IsLocal())
{
    app.UseHttpsRedirection();
}
app.UseAuthorization();

//Adding Middlewares
app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();
app.Run();

// In order to enable tests to run a test instance of a host
namespace BoredGames.API
{
    public partial class Program { }
}