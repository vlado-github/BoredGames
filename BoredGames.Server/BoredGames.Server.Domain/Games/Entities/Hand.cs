namespace BoredGames.Server.Domain.Games.Entities;

public class Hand
{
    public Hand(int roundNumber, string playerMove)
    {
        RoundNumber = roundNumber;
        PlayerMove = playerMove;
    }
    
    public int RoundNumber { get; set; }
    
    public string PlayerMove { get; set; }
}