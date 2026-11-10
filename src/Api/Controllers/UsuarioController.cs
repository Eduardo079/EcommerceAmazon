using System.Net;
using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Features.Auths.Users.Commands.LoginUser;
using Ecommerce.Application.Features.Auths.Users.Commands.RegisterUser;
using Ecommerce.Application.Features.Auths.Users.Vms;
using Ecommerce.Application.Models.ImageManagement;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/v1/[Controller]")]
public class UsuarioController : ControllerBase
{
    private IMediator _mediator;
    private IManageImageService _manageImageService;

    public UsuarioController(IMediator mediator, IManageImageService manageImageService)
    {
        _mediator = mediator;
        _manageImageService = manageImageService;
    }

    [AllowAnonymous]
    [HttpPost("login", Name = "Login")]
    [ProducesResponseType((int) HttpStatusCode.OK)]
    public async Task<ActionResult<AuthResponse>> Login ([FromBody] LoginUserCommands request)
    {
        return await _mediator.Send(request);
    }

    [AllowAnonymous]
    [HttpPost("register", Name = "Register")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<AuthResponse>> Register ([FromForm] RegisterUserCommand registerUser)
    {
        if(registerUser.Foto is not null)
        {
            ImageResponse resultImage = await _manageImageService.UploadImage(new ImageData
            {
                ImageStream = registerUser.Foto!.OpenReadStream(),
                Nombre = registerUser.Foto.Name
            });

            registerUser.FotoId = resultImage.PublicId;
            registerUser.FotoUrl = resultImage.Url;
        }

        return await _mediator.Send(registerUser);
    }
}