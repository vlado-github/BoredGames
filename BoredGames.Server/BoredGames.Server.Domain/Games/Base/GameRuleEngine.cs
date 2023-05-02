using BoredGames.Server.Domain.Games.Entities;

namespace BoredGames.Server.Domain.Games.Base;

public abstract class GameRuleEngine<T> where T: GameSettingsBase
{
    protected T _settings;
    protected Rounds _rounds;
    protected Score _score;

    public void Setup(T settings)
    {
        _settings = settings;
        _rounds = new Rounds(_settings.RequiredNumberOfWins);
        _score = new Score(_settings.RequiredNumberOfWins);
    }
}