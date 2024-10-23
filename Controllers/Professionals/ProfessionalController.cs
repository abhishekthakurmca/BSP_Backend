using Microsoft.AspNetCore.Mvc;
using MyBackendApp.Data;
using MyBackendApp.Models;

namespace MyBackendApp.Controllers.Professionals;

[ApiController]
[Route("api/[controller]")]
public class ProfessionalController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ProfessionalController> _logger;
    public ProfessionalController(ApplicationDbContext context, ILogger<ProfessionalController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // This will map to /professional/getProfessionals
    [HttpGet("getProfessionals")]
    public ActionResult<IEnumerable<Profession>> GetProfessionals()
    {
        try
        {
            var professionals = _context.Profession.ToList();
            return Ok(professionals);
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    // This will map to /professional/getServices
    [HttpGet("getServices")]
    public ActionResult<IEnumerable<Service>> GetServices()
    {
        var services = _context.Services.ToList();
        return Ok(services);
    }
}