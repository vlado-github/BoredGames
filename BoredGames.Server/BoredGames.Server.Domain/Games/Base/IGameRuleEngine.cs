using BoredGames.Server.Common.Enums;
using BoredGames.Server.Domain.Commands;
using BoredGames.Server.Domain.Games.Entities;

namespace BoredGames.Server.Domain.Games.Base;

public interface IGameRuleEngine<T> where T : GameSettingsBase
{
    void Setup(T settings);
    GameState Handle(MakeMoveCommand command);
    IList<Guid> GetWinners();
}