using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Features.Auths.Users.Vms;
using Ecommerce.Application.Identity;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Stripe.Terminal;

namespace Ecommerce.Application.Features.Auths.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResponse>
{
    private readonly UserManager<Usuario> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IAuthService _authService;

    public RegisterUserCommandHandler(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager, IAuthService authService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _authService = authService;
    }

    public async Task<AuthResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        bool existeUserByEmail = await _userManager.FindByEmailAsync(request.Email) is null ? false : true;

        if (existeUserByEmail)
            throw new BadRequestException("El email de usuario ya existe en la base de datos");

        bool existeUserByUserName = await _userManager.FindByNameAsync(request.UserName!) is null ? false : true;

        if(existeUserByUserName)
            throw new BadRequestException("El Username del usuario ya existe en la base de datos");

        Usuario usuario = new Usuario
        {
            Nombre = request.Nombre,
            Apellido = request.Apellido,
            Telefono = request.Telefono,
            Email = request.Email,
            UserName = request.UserName,
            AvatarUrl = request.FotoUrl
        };

        IdentityResult resultado = await _userManager.CreateAsync(usuario!, request.Password!);

        if (resultado.Succeeded)
        {
            await _userManager.AddToRoleAsync(usuario, AppRole.GenericUser);
            IList<string> roles = await _userManager.GetRolesAsync(usuario);

            return new AuthResponse
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Telefono = usuario.Email,
                Email = usuario.Email,
                UserName = usuario.UserName,
                Avatar = usuario.AvatarUrl,
                Token = _authService.CreateToken(usuario, roles),
                Roles = roles

            };
        }

        throw new Exception("No se pudo registrar el Usuario");
    }

}