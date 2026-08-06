using BoredGames.Server.Domain.Games.Dtos;
using BoredGames.Server.Domain.Games.Entities;

namespace BoredGames.Server.Domain.Games.Base;

public abstract class SimultaneousGameRuleEngine<T> : GameRuleEngine<T> where T : GameConfigurationBase
{
    public override RoundResult Handle(MoveDto dto)
    {
        _rounds.Current.AddMove(dto);
        if (_rounds.Current.GetMoves().Count == _settings.RequiredNumberOfPlayers)
        {
            return _gameSetup.ResultResolverAction();
        }

        return new RoundResult(
            roundStatus: _rounds.Current.GetStatus(), 
            roundNumber: _rounds.Current.Number);
    }
}