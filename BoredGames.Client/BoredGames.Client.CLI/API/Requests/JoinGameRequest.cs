namespace BoredGames.Client.CLI.API.Requests;

public class JoinGameRequest
{
    public Guid GameId { get; set; }
    public string? PlayerNickName { get; set; }
}