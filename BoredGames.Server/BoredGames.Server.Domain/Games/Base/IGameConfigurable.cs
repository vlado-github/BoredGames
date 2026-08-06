using BoredGames.Server.Domain.Games.Entities;

namespace BoredGames.Server.Domain.Games.Base;

public interface IGameConfigurable<in T> where T : GameConfigurationBase
{
    void Initialize();
}