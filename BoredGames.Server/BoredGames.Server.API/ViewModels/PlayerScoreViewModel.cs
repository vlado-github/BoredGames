namespace BoredGames.Server.API.ViewModels;

public class PlayerScoreViewModel
{
    public PlayerScoreViewModel()
    {
        RoundWins = new List<RoundResultViewModel>();
        RoundLosses = new List<RoundResultViewModel>();
        RoundDraws = new List<RoundResultViewModel>();
    }
    
    public Guid PlayerId { get; set; }
    public string PlayerNickName { get; set; }
    public IList<RoundResultViewModel> RoundWins { get; set; }
    public IList<RoundResultViewModel> RoundLosses { get; set; }
    public IList<RoundResultViewModel> RoundDraws { get; set; }
}