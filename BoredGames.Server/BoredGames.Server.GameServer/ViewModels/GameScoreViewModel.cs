namespace BoredGames.Server.GameServer.ViewModels;

[GenerateSerializer]
public class GameScoreViewModel
{
    public GameScoreViewModel()
    {
        PlayerScores = new List<PlayerScoreViewModel>();
    }
    
    [Id(0)]
    public int LastRound { get; set; }
    [Id(1)]
    public int RequiredNumberOfWins { get; set; }

    [Id(2)] 
    public IList<PlayerScoreViewModel> PlayerScores { get; set; }
}