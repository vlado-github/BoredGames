using BoredGames.Server.Common.Enums;
using BoredGames.Server.Domain.Commands;
using Orleans;

namespace BoredGames.Server.Domain.Grains.Base;

public interface IGameGrain : IGrainWithGuidKey
{
    Task<GameState> AddPlayerToGame(Guid playerId);
    Task<GameState> MakeMove(MakeMoveCommand command);
    Task<IList<Guid>> GetWinners();
}