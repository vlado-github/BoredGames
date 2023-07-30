using BoredGames.Server.Common.Enums;

namespace BoredGames.Server.Domain.Games.Entities;

public class RoundResult
{
    public RoundResult(RoundStatus roundStatus, int roundNumber)
    {
        RoundStatus = roundStatus;
        RoundNumber = roundNumber;
    }
    
    public RoundStatus RoundStatus { get; set; }
    public int RoundNumber { get; set; }
}