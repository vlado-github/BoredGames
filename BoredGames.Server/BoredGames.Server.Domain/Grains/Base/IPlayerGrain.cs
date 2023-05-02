using BoredGames.Server.Common.Enums;
using BoredGames.Server.Domain.Commands;
using BoredGames.Server.Domain.Games.Entities;
using Orleans;

namespace BoredGames.Server.Domain.Grains.Base;

public interface IPlayerGrain : IGrainWithGuidKey
{
    Task<GameDefinition> CreateGame(CreateGameCommand command);
    Task<GameDefinition> JoinGame(JoinGameCommand command);
}