using BoredGames.API.Models;
using BoredGames.Server.GameServer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace BoredGames.Server.Tests.ApiClients;

public interface IGameClient
{
    [Get("titles")]
    GameTitlesViewModel GetTitles();
    
    [Get("create")]
    Task<GameDefinitionViewModel> Create([FromBody] CreateGame request);
}