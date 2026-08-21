using BoredGames.Server.Domain.Games.Dtos;

namespace BoredGames.Server.GameServer.ViewModels;

[GenerateSerializer]
public class MoveViewModel
{
    [Id(0)]
    public Guid PlayerId { get; set; }
    [Id(1)]
    public string PlayerNickName { get; set; }
    [Id(2)]
    public string ActionType { get; set; }
    [Id(3)]
    public TilePositionViewModel SelectedTile { get; set; }
}

[GenerateSerializer]
public class TilePositionViewModel
{
    [Id(0)]
    public int Row { get; set; }
    [Id(1)]
    public int Column { get; set; }
}