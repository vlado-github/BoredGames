using System.Runtime.Serialization;
using Orleans;

namespace BoredGames.Server.Domain.Commands.Base;

[GenerateSerializer]
public class CommandBase
{
    [Id(0)]
    public Guid PlayerId { get; set; }
}