namespace BoredGames.Client.CLI.API.Responses;

public class GameStateResponse
{
    public Guid GameId { get; set; }
    public GameStatusEnum GameStatus { get; set; }
    public RoundStatusEnum RoundStatus { get; set; }
    public int RoundNumber { get; set; }
}