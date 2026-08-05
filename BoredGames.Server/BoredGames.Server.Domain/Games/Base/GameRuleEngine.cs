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
        _rounds = new Rounds(_settings.NumberOfRounds);
        _gameScore = new GameScore(_settings.NumberOfRounds, _settings.RequiredNumberOfWins);
    }

    public virtual RoundResult Handle(MoveDto dto)
    {
        _rounds.Current.AddMove(dto);
        if (_rounds.Current.GetMoves().Count == _settings.RequiredNumberOfPlayers)
        {
            return ResolveResult();
        }

        return new RoundResult(
            roundStatus: _rounds.Current.GetStatus(), 
            roundNumber: _rounds.Current.Number);
    }
    
    public abstract GameConfigurationBase GetDefinition();

    protected abstract RoundResult ResolveResult();
    
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