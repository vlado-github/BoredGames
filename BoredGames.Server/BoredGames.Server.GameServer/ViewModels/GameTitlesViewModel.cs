namespace BoredGames.Server.GameServer.ViewModels;

[GenerateSerializer]
public class GameTitlesViewModel
{
    [Id(0)] 
    public IList<GameTitleViewModel> Titles { get; set; } = new List<GameTitleViewModel>();
}