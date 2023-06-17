using BoredGames.Server.Domain.Commands;

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
        if (!Current.IsFinished)
        {
            Current.IsFinished = true;
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
}


public class Round
{
    public Round(int number, bool isCurrent = false)
    {
        IsCurrent = isCurrent;
        IsFinished = false;
        Number = number;
        MovesStack = new List<MakeMoveCommand>();
    }
    
    public void AddMove(MakeMoveCommand command)
    {
        MovesStack.Add(command);
    }

    public List<MakeMoveCommand> GetMoves()
    {
        return MovesStack;
    }

    public bool IsCurrent { get; set; }
    public bool IsFinished { get; set; }
    public int Number { get; private set; }
    public List<MakeMoveCommand> MovesStack { get; private set; }
}