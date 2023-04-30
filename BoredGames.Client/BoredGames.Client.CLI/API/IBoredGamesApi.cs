using BoredGames.Client.CLI.API.Requests;
using BoredGames.Client.CLI.API.Responses;
using Refit;

namespace BoredGames.Client.CLI.API;

[Headers("Accept: application/json")]
public interface IBoredGamesApi
{
    [Get("/api/game/{gameId}/state")]
    Task<GameStateResponse> GetGameState([AliasAs("gameId")] string gameId);

    [Get("/api/game/{gameId}/winners")]
    Task<List<Guid>> GetGameWinners([AliasAs("gameId")] string gameId);

    [Post("/api/game/create")]
    Task<Guid> CreateGame([Body] CreateGameRequest request);

    [Post("/api/game/makemove")]
    Task<GameStateResponse> MakeMove([Body] MakeMoveRequest request);

    [Put("/api/game/join")]
    Task<GameStateResponse> Join([Body] JoinGameRequest request);
}