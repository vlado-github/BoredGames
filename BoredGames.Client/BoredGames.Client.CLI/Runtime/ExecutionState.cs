using BoredGames.Client.CLI.API.Responses;

namespace BoredGames.Client.CLI.Runtime;

public class ExecutionState
{
    public Guid GameId { get; set; }
    public GameStatusEnum GameStatus { get; set; }
    public RoundStatusEnum RoundStatus { get; set; }
    public bool IsNewRound { get; set; }
    public int RoundNumber { get; set; }
    public int RequiredNumberOfPlayers { get; set; }
    public int RequiredNumberOfWins { get; set; }
    public string? Description { get; set; }
    public bool Joined { get; set; }
    public GameScoreResponse? GameScore { get; set; } = null;
    public string ActionType { get; set; }
    public string WaitingToJoinMessage { get; set; } = "Waiting for players to join...";
    public bool IsWaitingToJoinMessagePrinted { get; set; } = false;
    public string WaitingForMoveMessage { get; set; } = "Waiting for other players to make a move...";
    public bool IsWaitingForMoveMessagePrinted { get; set; } = false;
}