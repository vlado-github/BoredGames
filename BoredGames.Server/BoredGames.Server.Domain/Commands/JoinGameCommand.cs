using BoredGames.Server.Common.Enums;
using BoredGames.Server.Domain.Commands.Base;
using Orleans;

namespace BoredGames.Server.Domain.Commands;

[GenerateSerializer]
public class JoinGameCommand : CommandBase
{
    [Id(0)]
    public Guid GameId { get; set; }
}