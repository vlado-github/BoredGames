using BoredGames.Client.CLI.API;
using BoredGames.Client.CLI.API.Requests;
using BoredGames.Client.CLI.API.Responses;

namespace BoredGames.Client.CLI.Runtime;

public class PlayHandler
{
    private readonly IBoredGamesApi _boredGamesApi;

    public PlayHandler(IBoredGamesApi boredGamesApi)
    {
        _boredGamesApi = boredGamesApi;
    }

    public async Task<ExecutionState> Handle(ExecutionState executionState)
    {
        executionState.GameState = await _boredGamesApi.GetGameState(executionState.GameId.ToString());
        if (executionState.GameState.State == GameStateEnum.AwaitingPlayers)
        {
            if (!executionState.IsWaitingToJoinMessagePrinted)
            {
                Console.Write(executionState.WaitingToJoinMessage);
                executionState.IsWaitingToJoinMessagePrinted = true;
            }
            else
            {
                Thread.Sleep(2000);
                Console.Write(".");
            }
        }
        else if (executionState.GameState.State == GameStateEnum.InPlay)
        {
            if (!string.IsNullOrEmpty(executionState.ActionType))
            {
                if (!executionState.IsWaitingForMoveMessagePrinted)
                {
                    Console.Write(executionState.WaitingForMoveMessage);
                    executionState.IsWaitingForMoveMessagePrinted = true;
                }
                else
                {
                    Thread.Sleep(2000);
                    Console.Write(".");
                }
            }
            else
            {
                Console.WriteLine(string.Empty);
                Console.WriteLine("Choose your action [R]ock/[P]aper/[S]cissors:");
                var action = Console.ReadLine();
                if (action.Equals("R", StringComparison.InvariantCultureIgnoreCase))
                {
                    executionState.ActionType = "rock";
                }
                else if (action.Equals("P", StringComparison.InvariantCultureIgnoreCase))
                {
                    executionState.ActionType = "paper";
                }
                else if (action.Equals("S", StringComparison.InvariantCultureIgnoreCase))
                {
                    executionState.ActionType = "scissors";
                }
                else
                {
                    Console.WriteLine("Unrecognized action type.");
                }

                if (!string.IsNullOrEmpty(executionState.ActionType))
                {
                    executionState.GameState = await _boredGamesApi.MakeMove(
                        new MakeMoveRequest(executionState.GameId, executionState.ActionType));
                }
            }
        }

        return executionState;
    }
}