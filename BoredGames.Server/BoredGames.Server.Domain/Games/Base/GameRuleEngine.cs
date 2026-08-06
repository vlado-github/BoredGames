using BoredGames.Server.Domain.Games.Dtos;
using BoredGames.Server.Domain.Games.Entities;

namespace BoredGames.Server.Domain.Games.Base;

public abstract class GameRuleEngine<T> : IGameRuleEngine, IGameConfigurable<T> where T : GameConfigurationBase
{
    protected T _settings;
    protected Rounds _rounds;
    protected GameScore _gameScore;
    protected IGameSetupBuilder<T> _gameSetupBuilder;
    protected GameSetup<T> _gameSetup;

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
}