using BoredGames.Server.Domain.Games.Entities;

namespace BoredGames.Server.Domain.Games.Base;

public interface IGameSetupBuilder<T> where T : GameConfigurationBase
{
    IGameSetupBuilder<T> AddConfiguration(T gameConfiguration);
    IGameSetupBuilder<T> AddResultResolver(Func<RoundResult> resolver);
    GameSetup<T> Build();
}