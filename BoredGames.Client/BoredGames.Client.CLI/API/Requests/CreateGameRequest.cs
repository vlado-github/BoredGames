namespace BoredGames.Client.CLI.API.Requests;

public class CreateGameRequest
{
    public int GameTitle { get; set; }
    public int NumberOfPlayers { get; set; }
    public int NumberOfWins { get; set; }
    public string? Description { get; set; }
    public string? PlayerNickName { get; set; }
}