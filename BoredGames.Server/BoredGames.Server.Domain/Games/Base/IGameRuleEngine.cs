using BoredGames.Server.Domain.Games.Dtos;
using BoredGames.Server.Domain.Games.Entities;

namespace BoredGames.Server.Domain.Games.Base;

public interface IGameRuleEngine
{
    RoundResult Handle(MoveDto dto);
    IList<Player> GetWinners();
    GameScore GetScore();
    GameConfigurationBase GetDefinition();
    RoundResult GetCurrentRoundResult();
    bool AreAllRoundsFinished();
}