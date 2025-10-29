using FluentValidation;

namespace BoredGames.API.Models;

public class UpdatePlayerProfile
{
    public string Nickname { get; set; } = string.Empty;
}

public class AssignNicknameValidator : AbstractValidator<UpdatePlayerProfile>
{
    public AssignNicknameValidator()
    {
        RuleFor(x => x.Nickname).NotEmpty().NotNull();
    }
}