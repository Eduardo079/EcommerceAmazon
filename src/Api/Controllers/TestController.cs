using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Models.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.TestController;

[ApiController]
[Route("api/v1/[Controller]")]
public class TestController : ControllerBase
{
    private readonly IEmailService _emailService;
    public TestController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> SendEmail()
    {
        var message = new EmailMessage
        {
            To = "ecommerce.amazon.client@gmail.com",
            Body = "Esta es una prueba de envio de email con token",
            Subject = "Cambiar el password",
        };

        var result = await _emailService.SendEmailAsync(message, "Este_Es_Mi_Token");

        return result ? Ok() : BadRequest();
    }
}