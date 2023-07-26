using BoredGames.Server.Domain.Commands.Base;
using Orleans;

namespace BoredGames.Server.Domain.Commands;

[GenerateSerializer]
public class MakeMoveCommand : CommandBase
{
    [Id(0)]
    public Guid PlayerId { get; set; }
    [Id(1)]
    public string ActionType { get; set; }
    [Id(2)]
    public string PlayerNickName { get; set; }
}