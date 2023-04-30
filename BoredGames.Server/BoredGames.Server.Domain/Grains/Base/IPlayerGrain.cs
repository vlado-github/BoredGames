using BoredGames.Server.Common.Enums;
using BoredGames.Server.Domain.Commands;
using Orleans;

namespace BoredGames.Server.Domain.Grains.Base;

public interface IPlayerGrain : IGrainWithGuidKey
{
    Task<Guid> CreateGame(CreateGameCommand command);
    Task<GameState> JoinGame(Guid gameId);
}