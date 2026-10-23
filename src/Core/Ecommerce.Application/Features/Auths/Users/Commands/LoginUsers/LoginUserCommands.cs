using Ecommerce.Application.Features.Auths.Users.Vms;
using MediatR;

namespace Ecommerce.Application.Features.Auths.Users.Commands.LoginUser;

public class LoginUserCommands : IRequest<AuthResponse>
{
    public string? Email { get; set; }
    public string? Passwaord { get; set; }
}