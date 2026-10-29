using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{

    private readonly IUsersService _service = new UsersService();
    private readonly ILogger<UsersController> _logger; //logger
    private readonly IConfiguration _configuration; //Configuration
    
    public UsersController(ILogger<UsersController> logger, IConfiguration config)
    {
        _configuration = config;
        _logger = logger;
    }



    //Afficher un ordi en particulier
    [HttpGet("search/{login}")]
    public async Task<IActionResult> Get(string login)
    {
        var user = await _service.AfficherUtilisateurParLogin(login);
        return Ok(user);
    }

    //Afficher un ordi en particulier
    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var users = await _service.AfficherUtilisateurs();
        return Ok(users);
    }

    


}