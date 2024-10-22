using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using MyBackendApp.Models;

[ApiController]
[Route("api")]
public class ProfessionalController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProfessionalController(AppDbContext context)
    {
        _context = context;
    }

    // This will map to /professional/getprofessionals
    [HttpGet("getprofessionals")]
    public ActionResult<IEnumerable<Profession>> GetProfessionals()
    {
        try
        {
            var professionals = _context.Profession.ToList();
            return Ok(professionals);
        }
        catch (Exception ex)
        {
            // Log the error (you could use a logging framework here)
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    // This will map to /professional/getservices
    [HttpGet("getservices")]
    public ActionResult<IEnumerable<Service>> GetServices()
    {
        var myservices = _context.Services.ToList();
        return Ok(myservices);
    }
}