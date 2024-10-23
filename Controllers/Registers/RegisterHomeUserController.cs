using Microsoft.AspNetCore.Mvc;
using MyBackendApp.Models;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Data;


namespace MyBackendApp.Controllers.Registers;
[ApiController]
[Route("api/[controller]")]
public class RegisterController : ControllerBase
{
    private readonly IConfiguration _configuration;
    //private readonly IEmailService _emailService;

    public RegisterController(IConfiguration configuration)
    {
        _configuration = configuration;
       // _emailService = emailService;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] HomeUser model)
    {
        Console.WriteLine($"Arrived in Register");

        if (ModelState.IsValid)
        {
            var activationToken = Guid.NewGuid().ToString();

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Pwd);
            Console.WriteLine($"NON Hashed pwd is {model.Pwd}");
            Console.WriteLine($"hashed pwd is {hashedPassword}");

            using (var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (var command = new MySqlCommand("sp_RegisterHomeUserWithActivation", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("email", model.Email);
                    command.Parameters.AddWithValue("pwd", hashedPassword);
                    command.Parameters.AddWithValue("phone", model.Phone);
                    command.Parameters.AddWithValue("firstName", model.FirstName);
                    command.Parameters.AddWithValue("lastName", model.LastName);
                    command.Parameters.AddWithValue("postcode", model.Postcode);
                    command.Parameters.AddWithValue("activationToken", activationToken);

                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                }
            }

            //var activationLink = Url.Action(nameof(Activate), "Register", new { token = activationToken }, Request.Scheme);
            //await _emailService.SendActivationEmailAsync(model.Email, activationLink);

            return Ok(new { message = "User registered successfully. Please check your email to activate your account." });
        }

        return BadRequest(ModelState);
    }

    [HttpGet("activate")]
    public async Task<IActionResult> Activate(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return BadRequest("Invalid token.");
        }

        using (var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        {
            using (var command = new MySqlCommand("sp_ActivateUser", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ActivationToken", token);

                connection.Open();
                var rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected > 0)
                {
                    return Ok(new { message = "Account activated successfully!" });
                }
                else
                {
                    return BadRequest("Invalid token or account already activated.");
                }
            }
        }
    }
}