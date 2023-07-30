using BoredGames.Server.Common.Exceptions;
using BoredGames.Server.Domain.Games.Dtos;
using BoredGames.Server.Domain.Games.Entities;

namespace BoredGames.Server.Domain.Games.Base;

public abstract class GameRuleEngine<T> : IGameRuleEngine, IGameConfigurable<T> where T : GameConfigurationBase
{
    protected T _settings;
    protected Rounds _rounds;
    protected GameScore _gameScore;

    public void Setup(T settings) 
    {
        _settings = settings;
        _rounds = new Rounds(_settings.RequiredNumberOfWins);
        _gameScore = new GameScore(_settings.RequiredNumberOfWins);
    }

    public abstract RoundResult Handle(MoveDto dto);
    
    public abstract GameConfigurationBase GetConfiguration();
    
    public RoundResult GetCurrentRoundResult()
    {
        return new RoundResult(
            roundStatus: _rounds.Current.GetStatus(),
            roundNumber: _rounds.Current.Number);
    }

    public bool RequiredNumberOfWinsAchieved()
    {
        return _gameScore.PlayerStatistics
            .Any(x => x.NumberOfWins == _settings.RequiredNumberOfWins);
    }
    
    public bool AreAllRoundsFinished()
    {
        return _rounds.AreFinished();
    }

    public void AddExtraRound()
    {
        _rounds.AddExtraRound();
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