using BoredGames.Client.CLI.API.Responses;

namespace BoredGames.Client.CLI.Runtime;

public class ExecutionState
{
    public bool Joined { get; set; }
    public GameDefinitionResponse GameDefinition { get; set; }
    public GameStateResponse GameState { get; set; }
    public GameScoreResponse GameScore { get; set; }
    public string ActionType { get; set; }
    public string WaitingToJoinMessage { get; set; } = "Waiting for players to join...";
    public bool IsWaitingToJoinMessagePrinted { get; set; } = false;
    public string WaitingForMoveMessage { get; set; } = "Waiting for other players to make a move...";
    public bool IsWaitingForMoveMessagePrinted { get; set; } = false;
}