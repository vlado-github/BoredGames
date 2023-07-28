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

    public abstract IList<Player> GetWinners();

    public abstract GameScore GetScore();

    public abstract GameConfigurationBase GetConfiguration();
    public abstract RoundResult GetCurrentRoundResult();
    public abstract bool AreAllRoundsFinished();
}