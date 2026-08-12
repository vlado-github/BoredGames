using BoredGames.Common.Enums;
using BoredGames.Server.Domain.Games.Dtos;
using BoredGames.Server.Domain.Games.Entities;

namespace BoredGames.Server.Domain.Games.Base;

public class GameSetup<T>(
    T gameConfiguration, 
    Func<MoveDto, RoundResult> resultResolverAction,
    Action<ReadOnlyGameState>? gameStateHandlerAction = null)
    where T : GameConfigurationBase
{
    public readonly T GameConfiguration = gameConfiguration;
    public readonly Func<MoveDto, RoundResult> ResultResolverAction = resultResolverAction;
    public readonly Action<ReadOnlyGameState>? GameStateHandlerAction = gameStateHandlerAction;
}