using BoredGames.Common.Enums;
using BoredGames.Server.Domain.Games.Dtos;
using BoredGames.Server.Domain.Games.PubSub;

namespace BoredGames.Server.Domain.Games.Entities;

public class GameState
{
    public GameState()
    {
        GameStatus = GameStatus.AwaitingPlayers;
    }
    
    public Guid GameId { get; set; }
    public GameStatus GameStatus { get; set; }
    public RoundStatus RoundStatus { get; set; }
    public int RoundNumber { get; set; }
    public Guid? CurrentPlayerTurn { get; set; } = null;
    public Guid[]? PlayersTurnOrder {  get; set; } = null;
    public IList<PlayerDto> Players { get; set;  } = new List<PlayerDto>();
    public List<MoveDto> Moves { get; set;  } = new List<MoveDto>();
    
    public int PlayersNumber => Players.Count;
    
    public void ChangeGameStatus(GameStatus gameStatus, GameStateTracker gameStateTracker)
    {
        GameStatus = gameStatus;
        gameStateTracker.TrackGameState(this);
    }

    public void SyncRoundResult(RoundResult result)
    {
        AddMoves(result.Moves);
        RoundNumber = result.RoundNumber;
        RoundStatus = result.RoundStatus;
        CurrentPlayerTurn = result.CurrentPlayerTurn;
        PlayersTurnOrder = result.PlayersTurnOrder;
    }

    public ReadOnlyGameState AsReadOnly()
    {
        return new ReadOnlyGameState(GameId, GameStatus, RoundStatus, RoundNumber, Players, CurrentPlayerTurn, PlayersTurnOrder);
    }

    private void AddMoves(IList<MoveDto> roundMoved)
    {
        var additionalMoves = roundMoved.Except(Moves);
        Moves.AddRange(additionalMoves);
    }
}