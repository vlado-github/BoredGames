using BoredGames.Common.Enums;
using BoredGames.Server.Domain.Games.Dtos;
using BoredGames.Server.Domain.Games.Entities;

namespace BoredGames.Server.Domain.Games.Base;

public abstract class TurnBaseGameRuleEngine<T> : GameRuleEngine<T> where T : GameConfigurationBase
{
    protected Turns? Turns { get; private set; } = null;

    protected TurnBaseGameRuleEngine()
    {
        _gameSetupBuilder.AddGameStateHandler(InitializeTurnOrder);
    }
    
    public override RoundResult Handle(MoveDto moveDto)
    {
        _rounds.Current.AddMove(moveDto);
        if (Turns == null)
        {
            throw new Exception("Turn order not initialized");
        }
        Turns.TurnNext();
        return _gameSetup.ResultResolverAction(moveDto);
    }
    
    private void InitializeTurnOrder(GameState gameState)
    {
        if (Turns == null && gameState.GameStatus == GameStatus.InPlay)
        {
            Turns = new Turns(gameState.Players.Select(x => new Player(x.Id, x.NickName)).ToList());
        }
    }
}