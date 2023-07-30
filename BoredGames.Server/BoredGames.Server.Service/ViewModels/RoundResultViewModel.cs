using Orleans;

namespace BoredGames.Server.Service.ViewModels;

[GenerateSerializer]
public class RoundResultViewModel
{
    [Id(0)]
    public int RoundNumber { get; set; }
    [Id(1)]
    public string PlayerMove { get; set; }
}