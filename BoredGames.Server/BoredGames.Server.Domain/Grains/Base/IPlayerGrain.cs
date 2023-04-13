using BoredGames.Server.Common.Enums;
using Orleans;

namespace BoredGames.Server.Domain.Grains.Base;

public interface IPlayerGrain : IGrainWithGuidKey
{
    Task<Guid> CreateGame();
    Task<GameState> JoinGame(Guid gameId);
}