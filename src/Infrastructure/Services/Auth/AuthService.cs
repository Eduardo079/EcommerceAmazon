using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ecommerce.Application.Identity;
using Ecommerce.Application.Models.Token;
using Ecommerce.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Ecommerce.Infrastructure.Services.Auth;

    /// <summary>
    /// Servicio de autenticación encargado de crear tokens JWT y
    /// obtener datos del usuario autenticado desde el HttpContext.
    /// </summary>
public class AuthService : IAuthService
{
    /// <summary>
    /// Configuración del JWT (clave, expiración, issuer/audience si los usas).
    /// </summary>
    public JwtSettings _jwtSettings { get; }

        
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Ctor: inyecta HttpContextAccessor y JwtSettings (vía IOptions).
    /// </summary>
    public AuthService(IHttpContextAccessor httpContextAccessor, IOptions<JwtSettings> jwtSettings)
    {
        _httpContextAccessor = httpContextAccessor;
        _jwtSettings = jwtSettings.Value;
    }

    /// <summary>
        /// Crea un token JWT con claims básicos (username, userId, email y roles).
        /// </summary>
        /// <param name="usuario">Entidad con datos del usuario (Id, UserName, Email).</param>
        /// <param name="roles">Lista de roles del usuario (puede ser null).</param>
        /// <returns>JWT en formato compacto (string "eyJhbGciOi...").</returns>
    public string CreateToken(Usuario usuario, IList<string>? roles)
    {
        // Arma la lista de claims (datos que “metes” dentro del token).
        // NameId: aquí pones el UserName; también agregas Id y email.
        var claims = new List<Claim>
        {
             // NameId = identificador “legible” (aquí decides usar el username).
            new Claim(JwtRegisteredClaimNames.NameId, usuario.UserName!),
             // "UserId": claim personalizado con el Id del usuario.
            new Claim("UserId", usuario.Id),
            // "email": claim con el correo del usuario (podrías usar JwtRegisteredClaimNames.Email).
            new Claim("email", usuario.Email!)
        };

        // Agrega cada rol como un claim de tipo Role para que [Authorize(Roles="...")] funcione.
        foreach (var rol in roles!)      // ¡OJO! roles! asume que no es null → mejor null-check.
        {
            var claim = new Claim(ClaimTypes.Role, rol);
            claims.Add(claim);

        }

        // Deriva la clave simétrica desde el texto configurado (secreto compartido).
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key!));

        // Credenciales de firmado: algoritmo HMAC + clave simétrica.
        var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        // Descriptor del token: qué sujeto (claims), cuándo expira y con qué credenciales se firma.
        var tokenDescription = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),                   // Identidad con todos los claims.
            Expires = DateTime.UtcNow.Add(_jwtSettings.ExpireTime),  // Expiración (UTC + ventana configurada).
            SigningCredentials = credenciales                       // Firma del token.
        };

         // Manejador que crea y serializa el JWT.
        var tokenHandler = new JwtSecurityTokenHandler();

        // Construye el token (objeto SecurityToken en memoria).
        var token = tokenHandler.CreateToken(tokenDescription);

         // Lo escribe en formato compacto (string listo para Authorization: Bearer ...).
        return tokenHandler.WriteToken(token);
    }

    /// <summary>
    /// Obtiene el “usuario en sesión” (según el claim NameIdentifier mapeado en el contexto).
    /// </summary>
    public string GetSessionUser()
    {
        // Lee el HttpContext actual (request en curso).
            // Busca el primer claim cuyo Type sea NameIdentifier y devuelve su Value.
            // OJO: Dependiendo de tu configuración de mapeo de claims, NameId (JWT) puede no mapear a NameIdentifier.
        var username = _httpContextAccessor.HttpContext!.User.Claims?
        .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        return username!;   // ¡OJO! El “!” asume que no es null. Si puede ser null, maneja null-safe.
    }
}
