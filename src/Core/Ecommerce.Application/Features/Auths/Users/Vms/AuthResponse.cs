using Ecommerce.Application.Features.Addresses.Vms;

namespace Ecommerce.Application.Features.Auths.Users.Vms;

public class AuthResponse
{
    public string? Id { get; set; }
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string? Token { get; set; }
    public string? Avatar { get; set; }
    public AddressVms? DireccionEnvio { get; set; }
    public ICollection<string>? Roles { get; set; }
}