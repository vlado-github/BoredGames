using FluentValidation;

namespace BoredGames.API.Models;

public class CreatePlayerProfile
{
    public string Nickname { get; set; } = string.Empty;
}

public class CreatePlayerProfileValidator : AbstractValidator<CreatePlayerProfile>
{
    public CreatePlayerProfileValidator()
    {
        RuleFor(x => x.Nickname).NotEmpty().NotNull();
    }
}