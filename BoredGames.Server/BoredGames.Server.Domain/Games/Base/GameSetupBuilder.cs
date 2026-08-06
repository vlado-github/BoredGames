using BoredGames.Server.Domain.Games.Entities;

namespace BoredGames.Server.Domain.Games.Base;

public class GameSetupBuilder<T> : IGameSetupBuilder<T> where T : GameConfigurationBase
{
    private T _gameConfiguration;
    private Func<RoundResult> _resultResolverAction;
    
    public IGameSetupBuilder<T> AddConfiguration(T gameConfiguration)
    {
        _gameConfiguration = gameConfiguration;
        return this;
    }

    public IGameSetupBuilder<T> AddResultResolver(Func<RoundResult> resolver)
    {
        _resultResolverAction = resolver;
        return this;
    }

    public GameSetup<T> Build()
    {
        return new GameSetup<T>(_gameConfiguration, _resultResolverAction);
    }
}