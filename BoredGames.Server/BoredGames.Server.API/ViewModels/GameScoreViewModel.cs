namespace BoredGames.Server.API.ViewModels;

public class GameScoreViewModel
{
    public int CurrentRound { get; set; }
    public IList<PlayerScoreViewModel> PlayerScores { get; set; }
}