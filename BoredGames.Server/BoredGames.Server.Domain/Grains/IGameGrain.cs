using BoredGames.Server.Common.Enums;
using Orleans;

namespace BoredGames.Server.Domain.Grains;

public interface IGameGrain : IGrainWithGuidKey
{
    Task<GameState> AddPlayerToGame(Guid playerId);
}