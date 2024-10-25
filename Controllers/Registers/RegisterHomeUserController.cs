using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Data;
using MyBackendApp.Dto.Home;
using MyBackendApp.IServices;

namespace MyBackendApp.Controllers.Registers;
[ApiController]
[Route("api/[controller]")]
public class RegisterHomeUserController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IHomeUserService _homeUserService;

    public RegisterHomeUserController(IConfiguration configuration, IHomeUserService homeUserService)
    {
        _configuration = configuration;
        _homeUserService = homeUserService;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] HomeUserDto model)
    {
        if (ModelState.IsValid)
        {
            var response = await _homeUserService.RegisterUser(model);
            return Ok(response);
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

    [HttpPost("verify-activation")]
    public IActionResult VerifyActivationCode([FromBody] ActivationRequestDto request)
    {

        if (string.IsNullOrEmpty(request.ActivationCode))
            return BadRequest("Activation code is missing.");

        bool isValid = _homeUserService.ActivateUser(request);
        if (isValid)
            return Ok(new { message = "Activation successful" });

        return BadRequest("Invalid activation code.");
    }
}