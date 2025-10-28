namespace BoredGames.Server.GameServer.ViewModels;

[GenerateSerializer]
public class GameWinnersViewModel
{
    [Id(0)]
    public IList<PlayerViewModel> Winners { get; set; } = new List<PlayerViewModel>();
}