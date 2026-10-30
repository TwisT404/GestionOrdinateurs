using MySecureApi;
using Microsoft.AspNetCore.Mvc;

namespace MySecureApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KeycloakAuthController : ControllerBase
    {
        private readonly KeycloakAuthService _keycloakAuthService;

        public KeycloakAuthController(KeycloakAuthService keycloakAuthService)
        {
            _keycloakAuthService = keycloakAuthService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var token = await _keycloakAuthService.LoginAsync(request.Username, request.Password);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }

    public class LoginRequest
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}