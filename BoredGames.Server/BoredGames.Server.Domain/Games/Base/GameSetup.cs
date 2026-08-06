using BoredGames.Server.Domain.Games.Entities;

namespace BoredGames.Server.Domain.Games.Base;

public class GameSetup<T>(T gameConfiguration, Func<RoundResult> resultResolverAction)
    where T : GameConfigurationBase
{
    public readonly T GameConfiguration = gameConfiguration;
    public readonly Func<RoundResult> ResultResolverAction = resultResolverAction;
}