using Microsoft.AspNetCore.Mvc;
using MyBackendApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MyBackendApp.Data;
using MyBackendApp.Dto.Home;
using MyBackendApp.Utils;

namespace MyBackendApp.Controllers.Home;
[ApiController]
[Route("api/[controller]")]
public class HomeUsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    private readonly ILogger<HomeUsersController> _logger;
    private readonly IConfiguration _configuration;

    public HomeUsersController(ApplicationDbContext context, ILogger<HomeUsersController> logger, IConfiguration configuration)
    {
        _context = context;
        _logger = logger;
        _configuration = configuration;

    }

    // DO NOT EXPOSE THIS API
    /*
    [HttpGet("homeusers")]
    public async Task<ActionResult<IEnumerable<HomeUserModel>>> GetHomeUsers()
    {
        var users = await _context.HomeUsers.ToListAsync();
        return Ok(users);
    }

    */


    [HttpGet("homeUsers")]
    public async Task<ActionResult<IEnumerable<HomeUserDto>>> GetHomeUsersDTO()
    {
        var users = await _context.HomeUsers.Select(
            e => new HomeUserDto
            {
                UserId = e.user_id,
                Phone = e.Phone,
                Email = e.Email,
            }).ToListAsync();

        return Ok(users);
    }

    [HttpGet("getHomeUsersUsingSQL")]
    public async Task<ActionResult<IEnumerable<HomeUserDto>>> GetHomeUserSQL()
    {
        var sql = "SELECT user_id AS user_id, email AS email, phone AS phone FROM homeusers";
        var homeUsers = await _context.HomeUsers.FromSqlRaw(sql).Select(
            e => new HomeUserDto
            {
                UserId = e.user_id,
                Phone = e.Phone,
                Email = e.Email,
            }
        ).ToListAsync();
        return Ok(homeUsers);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        _logger.LogInformation("Login attempt for user {Email}", request.Email);

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid login attempt for user {Email}", request.Email);
            return BadRequest(ModelState);
        }

        var user = await _context.HomeUsers.SingleOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || !SharedResource.VerifyPassword(request.Password, user.Pwd))
        {
            _logger.LogWarning("Invalid login credentials for user {Email}", request.Email);
            return Unauthorized("Invalid credentials");
        }

        var token = GenerateJwtToken(user);
        _logger.LogInformation("Login successful for user {Email}", request.Email);

        return Ok(new { message = "Login successful", token });
    }

    private string GenerateJwtToken(HomeUser user)
    {
        try
        {
            // Retrieve the secret key from configuration
            var _secretKey = _configuration["JWTSettings:SecretKey"];

            //DELETE ME
            //_secretKey = "erEYtBnhaM3NO7+esan9ThcXOUtSlXgq4yE5CwAIF5Q=";

            var key = Encoding.UTF8.GetBytes(_secretKey);

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.user_id.ToString())
            }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while generating the JWT token.");
            throw;  
        }
    }
}


