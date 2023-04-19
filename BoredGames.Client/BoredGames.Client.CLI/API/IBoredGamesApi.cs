using BoredGames.Client.CLI.API.Responses;
using Refit;

namespace BoredGames.Client.CLI.API;

[Headers("Accept: application/json")]
public interface IBoredGamesApi
{
    [Get("/api/game/{gameId}/state")]
    Task<GameState> GetGameState([AliasAs("gameId")] string gameId);

    [Get("/api/game/{gameId}/winners")]
    Task<List<Guid>> GetGameWinners([AliasAs("gameId")] string gameId);
    
    
    
}