using BoredGames.Server.Domain.Games.Entities;

namespace BoredGames.Server.Domain.Games.Dtos;

public class MoveDto
{
    public Guid PlayerId { get; set; }
    public string PlayerNickName { get; set; }
    public string ActionType { get; set; }
    public TilePosition SelectedTile { get; set; }
}