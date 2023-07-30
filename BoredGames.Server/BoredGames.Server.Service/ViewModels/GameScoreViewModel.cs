using Orleans;

namespace BoredGames.Server.Service.ViewModels;

[GenerateSerializer]
public class GameScoreViewModel
{
    [Id(0)]
    public int CurrentRound { get; set; }
    [Id(1)]
    public int RequiredNumberOfWins { get; set; }
    [Id(2)]
    public IList<PlayerScoreViewModel> PlayerScores { get; set; }
}