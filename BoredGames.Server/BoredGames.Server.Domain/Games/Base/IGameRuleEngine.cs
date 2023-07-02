using BoredGames.Server.Common.Enums;
using BoredGames.Server.Domain.Commands;
using BoredGames.Server.Domain.Games.Entities;

namespace BoredGames.Server.Domain.Games.Base;

public interface IGameRuleEngine
{
    RoundResult Handle(MakeMoveCommand command);
    IList<Guid> GetWinners();
    GameScore GetScore();
    GameConfigurationBase GetConfiguration();
    RoundResult GetCurrentRoundResult();
    bool AreAllRoundsFinished();
}