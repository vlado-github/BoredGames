namespace BoredGames.Server.Domain.Games.Entities;

public class Hand
{
    public Hand(int roundNumber, string actionType)
    {
        RoundNumber = roundNumber;
        ActionType = actionType;
    }
    
    public int RoundNumber { get; set; }
    public string ActionType { get; set; }
}