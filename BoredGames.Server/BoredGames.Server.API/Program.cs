using BoredGames.Server.API.Extensions;
using BoredGames.Server.API.Filters;
using BoredGames.Server.API.Mappings;
using BoredGames.Server.API.Models;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Host.SetupOrleans();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(MakeMoveValidator));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCustomCors();
builder.Services.RegisterMappings();

var app = builder.Build();
if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(CorsPolicyExtensions.CorsPolicyName);
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

// In order to enable tests to run a test instance of a host
namespace BoredGames.Server.API
{
    public partial class Program { }
}