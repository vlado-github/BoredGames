using BoredGames.Server.Common.Enums;
using Orleans;

namespace BoredGames.Server.Domain.Games.Entities;

[GenerateSerializer]
public class RoundResult
{
    public RoundResult(RoundStatus roundStatus, int roundNumber)
    {
        RoundStatus = roundStatus;
        RoundNumber = roundNumber;
    }

    [Id(0)]
    public RoundStatus RoundStatus { get; set; }
    [Id(1)]
    public int RoundNumber { get; set; }
}