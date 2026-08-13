using BoredGames.Common.Enums;
using BoredGames.Server.Domain.Games.Dtos;
using BoredGames.Server.Domain.Games.PubSub;

namespace BoredGames.Server.Domain.Games.Entities;

public class GameState
{
    public GameState(Guid gameId, GameStatus gameStatus)
    {
        GameId = gameId;
        GameStatus = gameStatus;
    }
    
    public Guid GameId { get; private set; }
    public GameStatus GameStatus { get; private set; }
    public RoundStatus RoundStatus { get; set; }
    public int RoundNumber { get; set; }
    public Guid? CurrentPlayerTurn { get; set; } = null;
    public Guid[]? PlayersTurnOrder {  get; set; } = null;
    public int PlayersNumber => Players.Count;
    public IList<PlayerDto> Players { get; } = new List<PlayerDto>();
    public List<MoveDto> Moves { get; } = new List<MoveDto>();
    
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