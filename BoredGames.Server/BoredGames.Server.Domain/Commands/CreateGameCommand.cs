using BoredGames.Server.Domain.Commands.Base;
using Orleans;

namespace BoredGames.Server.Domain.Commands;

[GenerateSerializer]
public class CreateGameCommand : CommandBase
{
    [Id(0)]
    public int NumberOfPlayers { get; set; }
    [Id(1)]
    public int NumberOfWins { get; set; }
}