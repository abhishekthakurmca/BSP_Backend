using Microsoft.AspNetCore.Mvc;
using MyBackendApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MyBackendApp.Data;
using MyBackendApp.Dto.Business;
using MyBackendApp.Dto.Home;

namespace MyBackendApp.Controllers.Businesses;
[ApiController]
[Route("api/[controller]")]
public class BusinessUserController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<BusinessUserController> _logger;
    private readonly IConfiguration _configuration;

    public BusinessUserController(ApplicationDbContext context, ILogger<BusinessUserController> logger, IConfiguration configuration)
    {
        _context = context;
        _logger = logger;
        _configuration = configuration;
    }




    [HttpGet("businessUsersDto")]
    public async Task<ActionResult<IEnumerable<HomeUserDto>>> GetBusinessUserDTO()
    {
        var users = await _context.BusinessUser.Select(
            e => new BusinessUserDto
            {
                BusinessUserId = e.businessUserId,
                BusinessName = e.BusinessName,
                Phone = e.Phone,
                Email = e.Email,
            }
        ).ToListAsync();
        return Ok(users);
    }

    [HttpGet("getBusinessUsersUsingSQL")]
    public async Task<ActionResult<IEnumerable<BusinessUserDto>>> GetBusinessUserSQL()
    {
        var query = "SELECT businessuser_id AS businessuser_id, email AS email, phone AS phone FROM businessuser";
        var businessUsers = await _context.BusinessUser.FromSqlRaw(query).Select(
            bu => new BusinessUserDto
            {
                BusinessUserId = bu.businessUserId,
                Phone = bu.Phone,
                Email = bu.Email,
            }
        ).ToListAsync();

        return Ok(businessUsers);
    }


    [HttpPost("loginBusinessUser")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        _logger.LogInformation("Login attempt for user {Email}", request.Email);

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid login attempt for user {Email}", request.Email);
            return BadRequest(ModelState);
        }

        _logger.LogInformation("About to run SingleOrDefaultAsync for {Email}", request.Email);
        var user = await _context.BusinessUser.SingleOrDefaultAsync(u => u.Email == request.Email);

        if (user == null)
        {
            _logger.LogWarning("User not found for email {Email}", request.Email);
            return Unauthorized("Invalid credentials");
        }
        else
        {

            _logger.LogWarning("User found for email {Email}", user.Email);
        }

        // Do not log sensitive information like passwords
        _logger.LogInformation("User found, about to verify password for {Email}", request.Email);

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Pwd))
        {
            _logger.LogWarning("Invalid login credentials for user {Email}", request.Email);
            return Unauthorized("Invalid credentials");
        }

        var token = GenerateJwtToken(user);  // Assuming you have a method to generate JWT
        _logger.LogInformation("Login successful for user {Email}", request.Email);

        return Ok(new { message = "Login successful", token });
    }

    private string GenerateJwtToken(BusinessUserModel user)
    {
        try
        {
            // Retrieve the secret key from configuration
            var _secretKey = _configuration["JWTSettings:SecretKey"];
            // DELETE ME
            
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
                new Claim(ClaimTypes.NameIdentifier, user.businessUserId.ToString())
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


