using BoredGames.Server.GameServer.Commands;
using BoredGames.Server.GameServer.ViewModels;

namespace BoredGames.Server.GameServer.Grains.Base;

public interface IPlayerGrain : IGrainWithGuidKey
{
    Task<PlayerViewModel> GetDetails();
    Task<GameDefinitionViewModel> CreateGame(CreateGameCommand command);
    Task<PlayerViewModel> JoinGame(JoinGameCommand command);
}