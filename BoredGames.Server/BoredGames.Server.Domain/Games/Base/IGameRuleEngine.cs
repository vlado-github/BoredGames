using BoredGames.Server.Common.Enums;
using BoredGames.Server.Domain.Commands;

namespace BoredGames.Server.Domain.Games.Base;

public interface IGameRuleEngine
{
    GameState Handle(MakeMoveCommand command);
    IList<Guid> GetWinners();
}