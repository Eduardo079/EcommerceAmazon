using System.Configuration;
using Ecommerce.Application.Models.Email;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Application.Extensions;

public static class FluentEmailExtensions
{
    public static void AddServiceEmail(this IServiceCollection service, IConfiguration configuration)
    {
        service.Configure<EmailFluentSettings>
        (configuration.GetSection(nameof(EmailFluentSettings)));

        var emailSettings = configuration.GetSection(nameof(EmailFluentSettings));

        var fromEmail = emailSettings.GetValue<string>("Email");
        var host = emailSettings.GetValue<string>("Host");
        var port = emailSettings.GetValue<int>("Port");

        service.AddFluentEmail(fromEmail).AddSmtpSender(host, port);
    }
}