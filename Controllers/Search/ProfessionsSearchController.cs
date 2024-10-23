using Microsoft.AspNetCore.Mvc;
using MyBackendApp.Models;
using Microsoft.EntityFrameworkCore;
using MyBackendApp.Data;
using MyBackendApp.Dto.Profession;

namespace MyBackendApp.Controllers.Search;
[ApiController]
[Route("api/[controller]")]
public class ProfessionSearchController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProfessionSearchController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("searchCombo")]
    public async Task<IActionResult> GetProfessionsAndServices(string search)
    {

        if (string.IsNullOrEmpty(search))
            return BadRequest("Search term is required.");

        // Search both Professions and Services tables
        var professions = await _context.Profession
            .Where(p => p.Name.Contains(search))
            .Select(p => new { Id = p.ProfessionId, Name = p.Name, Type = "Profession" })
            .ToListAsync();

        var services = await _context.Services
            .Where(s => s.Name.Contains(search))
            .Select(s => new { Id = s.ServiceId, Name = s.Name, Type = "Service" })
            .ToListAsync();

        // Combine the results from both tables
        var combinedResults = professions.Concat(services)
            .OrderBy(r => r.Type) // Optional: Order results by name
            .ToList();

        return Ok(combinedResults);
    }


    [HttpGet("searchProfessions")]
    public async Task<IActionResult> GetProfessionsOnly(string search)
    {
        Console.WriteLine($"Arrived at searchProfessions");

        if (string.IsNullOrEmpty(search))
            return BadRequest("Search term is required.");

        // Search both Professions and Services tables
        var professions = await _context.Profession
            .Where(p => p.Name.Contains(search))
            .Select(p => new { Id = p.ProfessionId, Name = p.Name, Type = "Profession" })
            .ToListAsync();

        var ancillarys = await _context.ProfessionAncillary
            .Where(s => s.Name.Contains(search))
            .Select(s => new { Id = s.AncillaryId, Name = s.Name, Type = "Ancillary" })
            .ToListAsync();

        // Combine the results from both tables
        var combinedResults = professions.Concat(ancillarys)
            .OrderBy(r => r.Type) // Optional: Order results by name
            .ToList();

        return Ok(combinedResults);
    }



    [HttpGet("searchProfessionsOnly")]
    public async Task<IActionResult> GetProfessions(string search)
    {
        if (string.IsNullOrEmpty(search)) return BadRequest("Search term is required.");

        var professions = await _context.Profession
            .Where(p => p.Name.Contains(search))
            .Select(p => new { p.ProfessionId, p.Name })
            .ToListAsync();

        return Ok(professions);
    }



    [HttpPost]
    [Route("saveUserProfessions")]
    public IActionResult SaveUserProfessions([FromBody] UserProfessionsDto request)
    {
        foreach (var professionId in request.ProfessionIds)
        {
            _context.UserProfession.Add(new UserProfession
            {
                UserId = request.UserId,
                ProfessionId = professionId
            });
        }
        _context.SaveChanges();

        return Ok();
    }
}
