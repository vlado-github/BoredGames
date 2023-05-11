using System.Collections;
using BoredGames.Server.Common.Enums;
using BoredGames.Server.Domain.Commands;
using BoredGames.Server.Domain.Games.Base;
using BoredGames.Server.Domain.Games.Entities;
using Orleans;

namespace BoredGames.Server.Domain.Grains.Base;

public interface IGameGrain : IGrainWithGuidKey
{
    Task<GameDefinition> AddPlayerToGame(Guid playerId);
    Task<GameState> MakeMove(MakeMoveCommand command);
    Task<IList<Guid>> GetWinners();
    Task<GameState> GetState();
    Task<GameScore> GetScore();
    void Setup(CreateGameCommand command);
}