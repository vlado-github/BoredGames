using BoredGames.Server.GameServer.Commands.Base;

namespace BoredGames.Server.GameServer.Commands;

[GenerateSerializer]
public class JoinGameCommand : CommandBase
{
    [Id(0)]
    public Guid GameId { get; set; }

    [Id(1)] public string PlayerNickName { get; set; } = string.Empty;
}