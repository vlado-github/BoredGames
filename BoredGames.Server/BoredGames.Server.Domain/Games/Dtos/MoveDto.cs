namespace BoredGames.Server.Domain.Games.Dtos;

public class MoveDto
{
    public Guid PlayerId { get; set; }
    public string ActionType { get; set; }
    public string PlayerNickName { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }
}