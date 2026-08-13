using BoredGames.Server.Domain.Games.Entities;

namespace BoredGames.Server.Domain.Games.Dtos;

public class MoveDto : IEquatable<MoveDto>
{
    public Guid PlayerId { get; set; }
    public string PlayerNickName { get; set; }
    public string ActionType { get; set; }
    public TilePosition SelectedTile { get; set; }
    
    public bool Equals(MoveDto? other)
    {
        if (other is null)
        {
            return false;
        }

        return PlayerId == other.PlayerId
               && PlayerNickName == other.PlayerNickName
               && ActionType == other.ActionType
               && SelectedTile.Equals(other.SelectedTile);
    }
}