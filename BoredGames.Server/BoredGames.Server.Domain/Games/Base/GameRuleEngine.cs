using BoredGames.Server.Domain.Games.Dtos;
using BoredGames.Server.Domain.Games.Entities;
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
    private GameState _gameState; //todo: make readonly

    protected GameRuleEngine()
    {
        _gameSetupBuilder = new GameSetupBuilder<T>();
    }
    
    public void Initialize()
    {
        _gameSetup = _gameSetupBuilder.Build();
        _settings = _gameSetup.GameConfiguration;
        _rounds = new Rounds(_settings.NumberOfRounds);
        _gameScore = new GameScore(_settings.NumberOfRounds, _settings.RequiredNumberOfWins);
    }
    
    public abstract void Setup(T gameConfiguration);
    
    public abstract RoundResult Handle(MoveDto dto);
    
    public GameConfigurationBase GetDefinition()
    {
        return _settings;
    }
    
    public RoundResult GetCurrentRoundResult()
    {
        return new RoundResult(
            roundStatus: _rounds.Current.GetStatus(),
            roundNumber: _rounds.Current.Number);
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

    public virtual void Subscribe(IObservable<GameState> provider)
    {
        _unsubscriber = provider.Subscribe(this);
    }

    public virtual void OnCompleted()
    {
        Console.WriteLine("The GameState Tracker has completed transmitting data.");
        Unsubscribe();
    }

    public virtual void OnError(Exception e)
    {
        Console.WriteLine("The game state cannot be determined.");
    }

    public virtual void OnNext(GameState value)
    {
        _gameState = value;
        if (_gameSetup.GameStateHandlerAction != null)
        {
            _gameSetup.GameStateHandlerAction(value);
        }

        Console.WriteLine("The current game state is {0}", JsonConvert.SerializeObject(_gameState));
    }

    protected virtual void Unsubscribe()
    {
        _unsubscriber.Dispose();
    }
}