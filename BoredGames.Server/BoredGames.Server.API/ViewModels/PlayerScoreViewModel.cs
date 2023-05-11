namespace BoredGames.Server.API.ViewModels;

public class PlayerScoreViewModel
{
    public Guid PlayerId { get; set; }
    public IList<RoundResultViewModel> RoundWins { get; set; }
    public IList<RoundResultViewModel> RoundLosses { get; set; }
}