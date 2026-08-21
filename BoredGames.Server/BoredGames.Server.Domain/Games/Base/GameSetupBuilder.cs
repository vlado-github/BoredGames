using BoredGames.Common.Enums;
using BoredGames.Server.Domain.Games.Dtos;
using BoredGames.Server.Domain.Games.Entities;

namespace BoredGames.Server.Domain.Games.Base;

public class GameSetupBuilder<T> : IGameSetupBuilder<T> where T : GameConfigurationBase
{
    private T _gameConfiguration;
    private Func<MoveDto, RoundResult> _resultResolverAction;
    private Action<ReadOnlyGameState> _gameStateHandlerAction;
    private Action _roundCompletedHandlerAction;
    
    public IGameSetupBuilder<T> AddConfiguration(T gameConfiguration)
    {
        _gameConfiguration = gameConfiguration;
        return this;
    }

    public IGameSetupBuilder<T> AddResultResolver(Func<MoveDto, RoundResult> resolver)
    {
        _resultResolverAction = resolver;
        return this;
    }

    public IGameSetupBuilder<T> AddGameStateHandler(Action<ReadOnlyGameState> handler)
    {
        _gameStateHandlerAction = handler;
        return this;
    }

    public IGameSetupBuilder<T> AddRoundCompletedHandler(Action handler)
    {
        _roundCompletedHandlerAction = handler;
        return this;
    }

    public GameSetup<T> Build()
    {
        return new GameSetup<T>(_gameConfiguration, _resultResolverAction, _gameStateHandlerAction);
    }
}