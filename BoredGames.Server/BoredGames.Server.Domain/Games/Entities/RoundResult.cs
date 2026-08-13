using BoredGames.Common.Enums;
using BoredGames.Server.Domain.Games.Dtos;

namespace BoredGames.Server.Domain.Games.Entities;

public class RoundResult
{
    public RoundResult(
        RoundStatus roundStatus, 
        int roundNumber, 
        Guid? currentPlayerTurn = null,
        Guid[]? playersTurnOrder = null, 
        IList<MoveDto>? moves = null)
    {
        RoundStatus = roundStatus;
        RoundNumber = roundNumber;
        CurrentPlayerTurn = currentPlayerTurn;
        PlayersTurnOrder = playersTurnOrder;
        Moves = moves ?? new List<MoveDto>();
    }
    
    public RoundStatus RoundStatus { get; set; }
    public int RoundNumber { get; set; }
    public Guid? CurrentPlayerTurn { get; set; }
    public Guid[]? PlayersTurnOrder { get; set; }
    public IList<MoveDto> Moves { get; set; }
}