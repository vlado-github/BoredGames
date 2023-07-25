using BoredGames.Client.CLI.API.Requests;
using BoredGames.Client.CLI.API.Responses;
using Refit;

namespace BoredGames.Client.CLI.API;

[Headers("Accept: application/json")]
public interface IBoredGamesApi
{
    [Get("/api/game/{gameId}/state")]
    Task<GameStateResponse> GetGameState([AliasAs("gameId")] string gameId);
    
    [Get("/api/game/{gameId}/score")]
    Task<GameScoreResponse> GetGameScore([AliasAs("gameId")] string gameId);

    [Get("/api/game/{gameId}/winners")]
    Task<List<PlayerResponse>> GetGameWinners([AliasAs("gameId")] string gameId);

    [Post("/api/game/create")]
    Task<GameDefinitionResponse> CreateGame([Body] CreateGameRequest request);

    [Post("/api/game/makemove")]
    Task<GameStateResponse> MakeMove([Body] MakeMoveRequest request);

    [Put("/api/game/join")]
    Task<GameDefinitionResponse> Join([Body] JoinGameRequest request);
}