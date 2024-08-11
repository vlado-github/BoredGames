using Orleans;

namespace BoredGames.Server.Service.ViewModels;

[GenerateSerializer]
public class GameTitleViewModel
{
    [Id(0)]
    public int Id { get; set; }
    [Id(1)]
    public string Name { get; set; }
    [Id(2)]
    public string ThumbnailImageUrl { get; set; }
    [Id(3)]
    public string FormSchema { get; set; }
}