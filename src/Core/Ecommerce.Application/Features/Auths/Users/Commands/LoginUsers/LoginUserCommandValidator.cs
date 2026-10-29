using System.Data;
using FluentValidation;

namespace Ecommerce.Application.Features.Auths.Users.Commands.LoginUser;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommands>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Email)
        .NotEmpty().WithMessage("El Email no puede ser nulo");

        RuleFor(x => x.Password)
        .NotEmpty().WithMessage("El Pasword no puede ser nulo");
    }
}