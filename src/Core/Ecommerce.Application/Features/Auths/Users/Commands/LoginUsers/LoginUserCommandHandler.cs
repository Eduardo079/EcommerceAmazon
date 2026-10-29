using AutoMapper;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Features.Addresses.Vms;
using Ecommerce.Application.Features.Auths.Users.Vms;
using Ecommerce.Application.Identity;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Commands.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommands, AuthResponse>
{
    private readonly UserManager<Usuario> _userManager;
    private readonly SignInManager<Usuario> _singInManger;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public LoginUserCommandHandler(UserManager<Usuario> userManager, SignInManager<Usuario> singInManger, RoleManager<IdentityRole> roleManager, IAuthService authService, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _singInManger = singInManger;
        _roleManager = roleManager;
        _authService = authService;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthResponse> Handle(LoginUserCommands request, CancellationToken cancellationToken)
    {
        Usuario? user = await _userManager.FindByEmailAsync(request.Email);
        if(user is null)
        {
            throw new NotFoundException(nameof(Usuario), request.Email!);
        }

        if (!user.IsActive)
        {
            throw new Exception($"El usuario {request.Email} esta bloqueado, contacte al admin");
        }

        SignInResult resultado = await _singInManger.CheckPasswordSignInAsync(user,request.Password!, false);

        if (!resultado.Succeeded)
        {
            throw new Exception("Las credenciales del Usuario son incorrectas");
        }

        Address direcctorioEnvio = await _unitOfWork.Repository<Address>().GetEntityAsync(x => x.UserName == user.UserName);

        IList<string> roles = await _userManager.GetRolesAsync(user);

        AuthResponse authResponse = new AuthResponse
        {
            Id = user.Id,
            Nombre = user.Nombre,
            Apellido = user.Apellido,
            Telefono = user.Telefono,
            Email = user.Email,
            UserName = user.UserName,
            Avatar = user.AvatarUrl,
            DireccionEnvio = _mapper.Map<AddressVms>(direcctorioEnvio),
            Token = _authService.CreateToken(user,roles),
            Roles = roles
        };

        return authResponse;
    }
}