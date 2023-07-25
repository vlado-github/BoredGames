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
                Console.WriteLine($">>> {playerScore.PlayerId} [{playerScore.PlayerNickName}] " +
                                  $"Win/Loss/Draw: {playerScore.RoundWins.Count()} " +
                                  $"/ {playerScore.RoundLosses.Count()}" +
                                  $"/ {playerScore.RoundDraws.Count()}");
            }
        }
    }

    public async Task<ExecutionState> Handle(ExecutionState executionState)
    {
        var response = await _boredGamesApi.GetGameState(executionState.GameId.ToString());
        executionState.GameStatus = response.GameStatus;
        executionState.RoundStatus = response.RoundStatus;
        executionState.IsNewRound = response.RoundNumber > executionState.RoundNumber;
        executionState.RoundNumber = response.RoundNumber;

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
                if (executionState.IsNewRound)
                {
                    executionState.ActionType = string.Empty;
                }
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
                        await _boredGamesApi.MakeMove(
                            new MakeMoveRequest(executionState.GameId, executionState.ActionType));
                        
                    }
                }
            }
        }
        else if (executionState.GameStatus == GameStatusEnum.Finished)
        {
            executionState.GameScore = await _boredGamesApi.GetGameScore(
                executionState.GameId.ToString());
            PrintGameScore(executionState.GameScore);
        }
        else
        {
            Console.WriteLine($"GameStatus {executionState.GameStatus} is not supported.");
        }

        return executionState;
    }
}