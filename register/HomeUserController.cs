using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using MyBackendApp.Models;
using Microsoft.EntityFrameworkCore;

using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

[ApiController]
[Route("api/[controller]")]
public class HomeUsersController : ControllerBase
{
    private readonly AppDbContext _context;

    private readonly ILogger<HomeUsersController> _logger;
    private readonly IConfiguration _configuration;

    public HomeUsersController(AppDbContext context, ILogger<HomeUsersController> logger, IConfiguration configuration)
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


    [HttpGet("homeusersdto")]
    public async Task<ActionResult<IEnumerable<HomeUserDTOModel>>> GetHomeUsersDTO()
    {
        // specify context.HomeUsers to get from home users table
        var users = await _context.HomeUsers.Select(
            hu => new HomeUserDTOModel
            {
                user_id = hu.user_id,
                phone = hu.Phone,
                email = hu.Email,
                // Map other properties as needed
            }
        ).ToListAsync();
        
        
        return Ok(users);
    }

    [HttpGet("getHomeUsersUsingSQL")]
    public async Task<ActionResult<IEnumerable<HomeUserDTOModel>>> GetHomeUserSQL()
    {
        var sql = "SELECT user_id AS user_id, email AS email, phone AS phone FROM homeusers";
        var homeUsers = await _context.HomeUsers.FromSqlRaw(sql).Select(
            hu => new HomeUserDTOModel
            {
                user_id = hu.user_id,
                phone = hu.Phone,
                email = hu.Email,
                // Map other properties as needed
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

        _logger.LogInformation("About to run SingleOrDefaultAsync {Email}", request.Email);
        var user = await _context.HomeUsers.SingleOrDefaultAsync(u => u.Email == request.Email);
        _logger.LogInformation("Finished running SingleOrDefaultAsync {Email}", request.Email);

        _logger.LogInformation("Request pwd {Email}", request.Email);
        _logger.LogInformation("DB pwd {Email}", user.Pwd);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Pwd))
        {
            _logger.LogWarning("Invalid login credentials for user {Email}", request.Email);
            return Unauthorized("Invalid credentials");

        }

        var token = GenerateJwtToken(user);  // Assuming you have a method to generate JWT
        _logger.LogInformation("Login successful for user {Email}", request.Email);

        return Ok(new { message = "Login successful", token });
    }

private string GenerateJwtToken(HomeUserModel user)
{
    try
    {
        // Retrieve the secret key from configuration
        var _secretKey = _configuration["JWTSettings:SecretKey"];

        //DELETE ME
        _secretKey = "erEYtBnhaM3NO7+esan9ThcXOUtSlXgq4yE5CwAIF5Q=";

        // Ensure the key is of adequate length and securely encoded
        var key = Encoding.UTF8.GetBytes(_secretKey);
        
        // Initialize the token handler
        var tokenHandler = new JwtSecurityTokenHandler();
        
        // Define the token descriptor with claims and signing credentials
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
        
        // Create the token
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        // Return the serialized token
        return tokenHandler.WriteToken(token);
    }
    catch (Exception ex)
    {
        // Handle or log the exception
        _logger.LogError(ex, "An error occurred while generating the JWT token.");
        throw;  // Re-throw or handle the exception as needed
    }
}
}


