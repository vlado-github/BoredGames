using BoredGames.Server.Common.Enums;
using Orleans;

namespace BoredGames.Server.Domain.Games.Entities;

[GenerateSerializer]
public class RoundResult
{
    public RoundResult(GameStatus gameStatus, RoundStatus roundStatus, int roundNumber)
    {
        GameStatus = gameStatus;
        RoundStatus = roundStatus;
        RoundNumber = roundNumber;
    }
    
    [Id(0)]
    public GameStatus GameStatus { get; set; }
    [Id(1)]
    public RoundStatus RoundStatus { get; set; }
    [Id(2)]
    public int RoundNumber { get; set; }
}