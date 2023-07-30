using BoredGames.Server.Service.Commands.Base;
using Orleans;

namespace BoredGames.Server.Service.Commands;

[GenerateSerializer]
public class JoinGameCommand : CommandBase
{
    [Id(0)]
    public Guid GameId { get; set; }
    [Id(1)]
    public string? PlayerNickName { get; set; }
}