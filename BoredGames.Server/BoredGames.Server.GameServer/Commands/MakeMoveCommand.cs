using System;
using BoredGames.Server.GameServer.Commands.Base;
using Orleans;

namespace BoredGames.Server.GameServer.Commands;

[GenerateSerializer]
public class MakeMoveCommand : CommandBase
{
    [Id(0)]
    public Guid PlayerId { get; set; }
    [Id(1)]
    public string ActionType { get; set; }
    [Id(2)]
    public string? PlayerNickName { get; set; }
    [Id(3)]
    public int? SelectedTileRow { get; set; }
    [Id(4)]
    public int? SelectedTileColumn { get; set; }
}