using System;
using BoredGames.Server.GameServer.Commands.Base;
using Orleans;

namespace BoredGames.Server.GameServer.Commands;

[GenerateSerializer]
public class UpdatePlayerProfileCommand : CommandBase
{
    [Id(0)]
    public Guid Id { get; set; }
    
    [Id(1)]
    public string Nickname { get; set; }
}