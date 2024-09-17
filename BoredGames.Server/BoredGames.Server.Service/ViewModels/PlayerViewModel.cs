using Orleans;

namespace BoredGames.Server.Service.ViewModels;

[GenerateSerializer]
public class PlayerViewModel
{
    [Id(0)]
    public Guid Id { get; set; }

    [Id(1)] 
    public string NickName { get; set; } = string.Empty;
}