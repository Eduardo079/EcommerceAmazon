using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Identity;
using Ecommerce.Application.Models.Email;
using Ecommerce.Application.Models.ImageManagement;
using Ecommerce.Application.Models.Token;
using Ecommerce.Application.Persistence;
using Ecommerce.Infrastructure.MessageImplementation;
using Ecommerce.Infrastructure.Persistence.Repositories;
using Ecommerce.Infrastructure.Services.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendGrid.Helpers.Mail;
using Stripe;

namespace Ecommerce.Infrastructure.Persistence;

public static class InfrastrutureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<IAuthService, AuthService>();

        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

        return services;
    }
}