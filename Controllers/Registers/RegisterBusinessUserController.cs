using Microsoft.AspNetCore.Mvc;
using MyBackendApp.Models;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Data;
using MyBackendApp.Services;

namespace MyBackendApp.Controllers.Registers;
[ApiController]
[Route("api/[controller]")]
public class RegisterBusinessUserController : ControllerBase
{
    private readonly IConfiguration _configuration;
    //private readonly IEmailService _emailService;

    public RegisterBusinessUserController(IConfiguration configuration, IEmailService emailService)
    {
        _configuration = configuration;
        // _emailService = emailService;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterBusinessUser([FromBody] BusinessUserModel model)
    {
        if (ModelState.IsValid)
        {
            var activationToken = Guid.NewGuid().ToString();

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Pwd);
            //Console.WriteLine($"NON Hashed pwd is {model.Pwd}");
            //Console.WriteLine($"hashed pwd is {hashedPassword}");
            Console.WriteLine($"BUSINESS NAME IS {model.BusinessName}");

            using (var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (var command = new MySqlCommand("sp_RegisterBusinessUserWithActivation", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_email", model.Email);
                    command.Parameters.AddWithValue("p_pwd", hashedPassword);
                    command.Parameters.AddWithValue("p_phone", model.Phone);
                    command.Parameters.AddWithValue("p_firstName", model.FirstName);
                    command.Parameters.AddWithValue("p_lastName", model.LastName);
                    command.Parameters.AddWithValue("p_postcode", model.Postcode);
                    command.Parameters.AddWithValue("p_activationToken", activationToken);
                    command.Parameters.AddWithValue("p_business_name", model.BusinessName);

                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                }
            }

            //var activationLink = Url.Action(nameof(Activate), "Register", new { token = activationToken }, Request.Scheme);
            //await _emailService.SendActivationEmailAsync(model.Email, activationLink);

            return Ok(new { message = "Business User registered successfully. Please check your email to activate your account." });
        }

        return BadRequest(ModelState);
    }

    [HttpGet("activateBusinessUser")]
    public async Task<IActionResult> ActivateBusinessUser(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return BadRequest("Invalid token.");
        }

        using (var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        {
            using (var command = new MySqlCommand("sp_ActivateBusinessUser", connection))
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