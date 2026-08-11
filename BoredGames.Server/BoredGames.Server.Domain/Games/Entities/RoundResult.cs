using BoredGames.Common.Enums;

namespace BoredGames.Server.Domain.Games.Entities;

public class RoundResult
{
    public RoundResult(RoundStatus roundStatus, int roundNumber, Guid? currentPlayerTurn = null, Guid? nextPlayerTurn = null)
    {
        RoundStatus = roundStatus;
        RoundNumber = roundNumber;
        CurrentPlayerTurn = currentPlayerTurn;
        NextPlayerTurn = nextPlayerTurn;
    }
    
    public RoundStatus RoundStatus { get; set; }
    public int RoundNumber { get; set; }
    public Guid? CurrentPlayerTurn { get; set; }
    public Guid? NextPlayerTurn { get; set; }
}