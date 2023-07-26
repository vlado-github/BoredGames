using Orleans;

namespace BoredGames.Server.Domain.Games.Entities;

[GenerateSerializer]
public class Hand
{
    public Hand(int roundNumber, string actionType)
    {
        RoundNumber = roundNumber;
        ActionType = actionType;
    }
    
    [Id(0)]
    public int RoundNumber { get; set; }

    [Id(1)]
    public string ActionType { get; set; }
}