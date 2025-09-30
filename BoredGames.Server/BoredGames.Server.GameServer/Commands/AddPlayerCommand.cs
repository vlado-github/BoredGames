using BoredGames.Server.GameServer.Commands.Base;

namespace BoredGames.Server.GameServer.Commands;

[GenerateSerializer]
public class AddPlayerCommand : CommandBase
{
    [Id(0)]
    public Guid Id { get; set; }
    [Id(1)]
    public string? NickName { get; set; }
}