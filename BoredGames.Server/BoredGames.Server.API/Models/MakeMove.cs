using FluentValidation;

namespace BoredGames.Server.API.Models;

public class MakeMove
{
    public Guid GameId { get; set; }
    public string ActionType { get; set; }
}

public class MakeMoveValidator : AbstractValidator<MakeMove>
{
    public MakeMoveValidator()
    {
        RuleFor(x => x.GameId).NotEmpty().NotNull().NotEqual(new Guid());
        RuleFor(x => x.ActionType).NotEmpty().NotNull();
    }
}