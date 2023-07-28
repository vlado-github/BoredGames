using Orleans;

namespace BoredGames.Server.Service.ViewModels;

[GenerateSerializer]
public class GameScoreViewModel
{
    public int CurrentRound { get; set; }
    public int RequiredNumberOfWins { get; set; }
    public IList<PlayerScoreViewModel> PlayerScores { get; set; }
}