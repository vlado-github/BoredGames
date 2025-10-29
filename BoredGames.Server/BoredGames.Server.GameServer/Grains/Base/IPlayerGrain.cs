using BoredGames.Server.GameServer.Commands;
using BoredGames.Server.GameServer.ViewModels;

namespace BoredGames.Server.GameServer.Grains.Base;

public interface IPlayerGrain : IGrainWithGuidKey
{
    Task<PlayerViewModel> GetProfile();
    Task<PlayerViewModel> UpdatePlayerProfile(UpdatePlayerProfileCommand command);
    Task<GameDefinitionViewModel> CreateGame(CreateGameCommand command);
    Task<GameStateViewModel> JoinGame(JoinGameCommand command);
}