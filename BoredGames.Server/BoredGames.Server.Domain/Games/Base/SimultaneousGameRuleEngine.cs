using BoredGames.Server.Domain.Games.Dtos;
using BoredGames.Server.Domain.Games.Entities;
using Microsoft.Extensions.Logging;

namespace BoredGames.Server.Domain.Games.Base;

public abstract class SimultaneousGameRuleEngine<T> : GameRuleEngine<T> where T : GameConfigurationBase
{
    protected SimultaneousGameRuleEngine(ILogger<SimultaneousGameRuleEngine<T>> logger) : base(logger)
    {
    }

    public override RoundResult Handle(MoveDto moveDto)
    {
        _rounds.Current.AddMove(moveDto);
        if (_rounds.Current.GetMoves().Count == _settings.RequiredNumberOfPlayers)
        {
            return _gameSetup.ResultResolverAction(moveDto);
        }

        return new RoundResult(
            roundStatus: _rounds.Current.GetStatus(), 
            roundNumber: _rounds.Current.Number);
    }
}