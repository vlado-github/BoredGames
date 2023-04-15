using BoredGames.Server.Domain.Commands.Base;
using Orleans;

namespace BoredGames.Server.Domain.Commands;

[GenerateSerializer]
public class MakeMoveCommand : CommandBase
{
    [Id(0)]
    public string ActionType { get; set; }
}