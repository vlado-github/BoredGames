using BoredGames.Server.Domain.Commands.Base;

namespace BoredGames.Server.Domain.Commands;

public class MakeMoveCommand : CommandBase
{
    public string ActionType { get; set; }
}