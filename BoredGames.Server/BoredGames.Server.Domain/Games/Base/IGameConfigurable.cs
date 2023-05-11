namespace BoredGames.Server.Domain.Games.Base;

public interface IGameConfigurable<T> where T : GameConfigurationBase
{
    void Setup(T configuration);
}