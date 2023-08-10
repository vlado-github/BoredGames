using BoredGames.Server.Common.Enums;
using BoredGames.Server.Common.Exceptions;
using BoredGames.Server.Domain.Games.Dtos;

namespace BoredGames.Server.Domain.Games.Entities;

public class Rounds
{
    private readonly IList<Round> _rounds;
    public Round Current;

    public Rounds(int numberOfRounds)
    {
        _rounds = new List<Round>();
        for (int i = 1; i <= numberOfRounds; i++)
        {
            var roundNumber = i;
            if (i == 1)
            {
                _rounds.Add(new Round(roundNumber, isCurrent: true));
            }
            else
            {
                _rounds.Add(new Round(roundNumber, isCurrent: false));
            }
        }
        Current = _rounds.Single(r => r.IsCurrent);
    }

    

    public void Next()
    {
        if (Current.IsFinished)
        {
            Current.IsCurrent = false;
            var nextNumber = Current.Number + 1;
            if (_rounds.Count() >= nextNumber)
            {
                var nextRound = _rounds.Single(r => r.Number == nextNumber);
                nextRound.IsCurrent = true;
                Current = nextRound;
            }
        }
    }

    public bool AreFinished()
    {
        return _rounds.All(x => x.IsFinished);
    }

    public void AddExtraRound()
    {
        if (!AreFinished())
        {
            throw new OperationNotAllowedException(
                $"Extra round is not allowed until all rounds are finished.");
        }
        _rounds.Add(new Round(_rounds.Count + 1));
    }
}


public class Round
{
    public Round(int number, bool isCurrent = false)
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
        if (MovesStack.Any(x => x.PlayerId == dto.PlayerId))
        {
            throw new OperationNotAllowedException(
                $"Player with ID {dto.PlayerId} has already made a move for round {Number}.");
        }
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