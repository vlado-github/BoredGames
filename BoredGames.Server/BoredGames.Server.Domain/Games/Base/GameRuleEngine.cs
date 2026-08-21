using BoredGames.Server.Domain.Games.Dtos;
using BoredGames.Server.Domain.Games.Entities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BoredGames.Server.Domain.Games.Base;

public abstract class GameRuleEngine<T> : 
    IGameRuleEngine, 
    IGameConfigurable<T> where T : GameConfigurationBase
{
    protected T _settings;
    protected Rounds _rounds;
    protected GameScore _gameScore;
    protected IGameSetupBuilder<T> _gameSetupBuilder;
    protected GameSetup<T> _gameSetup;
    
    private IDisposable _unsubscriber;
    private ReadOnlyGameState _gameState;
    private readonly ILogger<GameRuleEngine<T>> _logger;

    protected GameRuleEngine(ILogger<GameRuleEngine<T>> logger)
    {
        _logger = logger;
        _gameSetupBuilder = new GameSetupBuilder<T>();
    }
    
    public void Initialize()
    {
        _gameSetup = _gameSetupBuilder.Build();
        _settings = _gameSetup.GameConfiguration;
        _rounds = new Rounds(_settings.NumberOfRounds);
        _gameScore = new GameScore(_settings.NumberOfRounds, _settings.RequiredNumberOfWins);
    }
    
    public abstract void Setup(T gameConfiguration, Action? onRoundCompleted = null);

    public abstract RoundResult GetCurrentRoundResult();
    
    public abstract RoundResult Handle(MoveDto dto);
    
    public GameConfigurationBase GetDefinition()
    {
        return _settings;
    }

    public bool AreAllRoundsFinished()
    {
        return _rounds.AreFinished();
    }

    public GameScore GetScore()
    {
        return _gameScore;
    }

    public IList<Player> GetWinners()
    {
        return _gameScore.GetWinners();
    }

    public virtual void OnCompleted()
    {
        _logger.LogInformation("The GameState Tracker has completed transmitting data.");
        Unsubscribe();
    }

    public virtual void OnError(Exception e)
    {
        _logger.LogError("The game state cannot be determined.");
    }

    public virtual void OnNext(GameState value)
    {
        _gameState = value.AsReadOnly();
        if (_gameSetup.GameStateHandlerAction != null)
        {
            _gameSetup.GameStateHandlerAction(_gameState);
        }

        _logger.LogDebug("The current game state is {0}", JsonConvert.SerializeObject(_gameState));
    }

    protected virtual void Unsubscribe()
    {
        _unsubscriber.Dispose();
    }
}