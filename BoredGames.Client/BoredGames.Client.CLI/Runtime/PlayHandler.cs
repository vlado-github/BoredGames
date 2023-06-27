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

    private void PrintGameScore(GameScoreResponse? score)
    {
        if (score != null)
        {
            Console.WriteLine("");
            Console.WriteLine($">>> Round {score.CurrentRound}/{score.RequiredNumberOfWins}");
            foreach (var playerScore in score.PlayerScores)
            {
                Console.WriteLine($">>> {playerScore.PlayerId} " +
                                  $"Win/Loss: {playerScore.RoundWins.Count()} / {playerScore.RoundLosses.Count()}");
            }
        }
    }

    public async Task<ExecutionState> Handle(ExecutionState executionState)
    {
        var response = await _boredGamesApi.GetGameState(executionState.GameId.ToString());
        executionState.GameStatus = response.GameStatus;
        executionState.RoundStatus = response.RoundStatus;
        executionState.RoundNumber = response.RoundNumber;
        
        Console.WriteLine($">>> {executionState.RoundNumber} {executionState.RoundStatus}");
        
        if (executionState.GameStatus == GameStatusEnum.AwaitingPlayers)
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
        else if (executionState.GameStatus == GameStatusEnum.InPlay)
        {
            if (executionState.RoundStatus == RoundStatusEnum.InProgress)
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
                    var action = Console.ReadLine()?.First().ToString();
                    if (!string.IsNullOrEmpty(action))
                    {
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
                    }
                    else
                    {
                        Console.WriteLine("Unrecognized action type.");
                    }

                    if (!string.IsNullOrEmpty(executionState.ActionType))
                    {
                        var moveResponse = await _boredGamesApi.MakeMove(
                            new MakeMoveRequest(executionState.GameId, executionState.ActionType));
                        executionState.GameStatus = moveResponse.GameStatus;
                        executionState.RoundStatus = moveResponse.RoundStatus;
                        executionState.RoundNumber = moveResponse.RoundNumber;
                        
                        Console.WriteLine($">>>{executionState.RoundStatus}");

                        if (executionState.RoundStatus == RoundStatusEnum.Completed)
                        {
                            executionState.GameScore = await _boredGamesApi.GetGameScore(
                                executionState.GameId.ToString());
                            executionState.ActionType = string.Empty;
                            PrintGameScore(executionState.GameScore);
                        }
                    }
                }
            }
            else
            {
                executionState.GameScore = await _boredGamesApi.GetGameScore(
                    executionState.GameId.ToString());
                executionState.ActionType = string.Empty;
                PrintGameScore(executionState.GameScore);
            }
        }
        else if (executionState.GameStatus == GameStatusEnum.Finished)
        {
            executionState.GameScore = await _boredGamesApi.GetGameScore(
                executionState.GameId.ToString());
            PrintGameScore(executionState.GameScore);
        }
        

        return executionState;
    }
}