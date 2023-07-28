using Orleans;

namespace BoredGames.Server.Service.ViewModels;

[GenerateSerializer]
public class PlayerScoreViewModel
{
    public PlayerScoreViewModel()
    {
        RoundWins = new List<RoundResultViewModel>();
        RoundLosses = new List<RoundResultViewModel>();
        RoundDraws = new List<RoundResultViewModel>();
    }
    
    [Id(0)]
    public Guid PlayerId { get; set; }
    [Id(1)]
    public string PlayerNickName { get; set; }
    [Id(2)]
    public IList<RoundResultViewModel> RoundWins { get; set; }
    [Id(3)]
    public IList<RoundResultViewModel> RoundLosses { get; set; }
    [Id(4)]
    public IList<RoundResultViewModel> RoundDraws { get; set; }
}