using BoredGames.Common.Enums;
using BoredGames.Common.Exceptions;
using BoredGames.Server.Domain.Games.Dtos;

namespace BoredGames.Server.Domain.Games.Entities;

public class Round
{
    public Round(int number = 1, bool isCurrent = false)
    {
        IsCurrent = isCurrent;
        IsFinished = false;
        Number = number;
        MovesStack = new List<MoveDto>();
    }
    
    public void Complete()
    {
        IsFinished = true;
    }
    
    public void AddMove(MoveDto dto)
    {
        MovesStack.Add(dto);
    }

    public List<MoveDto> GetMoves()
    {
        return MovesStack;
    }

    public RoundStatus GetStatus()
    {
        var state = IsFinished ? RoundStatus.Completed : RoundStatus.InProgress;
        return state;
    }

    public bool IsCurrent { get; set; }
    public bool IsFinished { get; set; }
    public int Number { get; private set; }
    public List<MoveDto> MovesStack { get; private set; }
}