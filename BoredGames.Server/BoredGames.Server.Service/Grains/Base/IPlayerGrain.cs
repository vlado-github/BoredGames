using BoredGames.Server.Service.Commands;
using BoredGames.Server.Service.ViewModels;
using Orleans;

namespace BoredGames.Server.Service.Grains.Base;

public interface IPlayerGrain : IGrainWithGuidKey
{
    Task<string> GetNickName();
    Task<GameDefinitionViewModel> CreateGame(CreateGameCommand command);
    Task<GameDefinitionViewModel> JoinGame(JoinGameCommand command);
}