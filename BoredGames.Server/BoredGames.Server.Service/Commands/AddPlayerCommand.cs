using BoredGames.Server.Service.Commands.Base;
using Orleans;

namespace BoredGames.Server.Service.Commands;

[GenerateSerializer]
public class AddPlayerCommand : CommandBase
{
    [Id(0)]
    public Guid Id { get; set; }
    [Id(1)]
    public string? NickName { get; set; }
}