using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GestionOrdinateurs.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api/[controller]")]
public class AuthentificationController : ControllerBase
{
    //Attributs
    private readonly IConfiguration _config;
    private readonly IUserService _service;

    //Constructeur
    public AuthentificationController(IConfiguration config, IUserService service)
    {
        _config = config;
        _service = service;
    }

    //Création d'un token si login existant
    [HttpPost]
    public async Task<IActionResult> Login(string login)
    {
        var user = await _service.GetUserAsync(login);
        if (user is null)
        {
            return Unauthorized("Utilisateur non authentifié !");
        }
        
        //Config Token
        var jwtKey = _config["Jwt:Key"] ?? "";
        var key = Encoding.ASCII.GetBytes(jwtKey);

        //Génération Token
        var token = GenTokenJWT(key, user.Role, user.Login); 
        
        return Ok(token);
    }

    /// <summary>
    /// Permet de générer un token JWT
    /// </summary>
    /// <param name="keyP"></param>
    /// <returns></returns>
    private string GenTokenJWT(byte[] keyP, string role, string userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = new[] {
            new Claim(ClaimTypes.Name, userId),
            new Claim(ClaimTypes.Role, role)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyP), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);
        return jwt;
    }
}