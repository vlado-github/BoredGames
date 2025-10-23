using FluentValidation;

namespace BoredGames.API.Models;

public class MakeMove
{
    public string GameId { get; set; }
    public string PlayerId { get; set; }
    public string ActionType { get; set; }
}

public class MakeMoveValidator : AbstractValidator<MakeMove>
{
    public MakeMoveValidator()
    {
        RuleFor(x => x.GameId).NotEmpty().NotNull();
        RuleFor(x => x.PlayerId).NotEmpty().NotNull();
        RuleFor(x => x.ActionType).NotEmpty().NotNull();
    }
}