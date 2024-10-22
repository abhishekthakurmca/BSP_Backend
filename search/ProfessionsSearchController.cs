using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using MyBackendApp.Models;
using MyBackendApp.Models.HomeProfile;
using Microsoft.EntityFrameworkCore;

using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Asn1.Iana;


[ApiController]
[Route("api/[controller]")]
public class ProfessionController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProfessionController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("searchcombo")]
    public async Task<IActionResult> GetProfessionsAndServices(string search) 
    {

    if (string.IsNullOrEmpty(search))
        return BadRequest("Search term is required.");

        // Search both Professions and Services tables
        var professions =  await _context.Profession
            .Where(p => p.Name.Contains(search))
            .Select(p => new { Id = p.ProfessionId, Name = p.Name, Type = "Profession" })
            .ToListAsync();

        var services =  await _context.Services
            .Where(s => s.Name.Contains(search))
            .Select(s => new { Id = s.ServiceId, Name = s.Name, Type = "Service" })
            .ToListAsync();

        // Combine the results from both tables
        var combinedResults = professions.Concat(services)
            .OrderBy(r => r.Type) // Optional: Order results by name
            .ToList();

        return Ok(combinedResults);
    }


    [HttpGet("searchprofessions")]
    public async Task<IActionResult> GetProfessionsOnly(string search) 
    {
        Console.WriteLine($"Arived at searchprofessions");

    if (string.IsNullOrEmpty(search))
        return BadRequest("Search term is required.");

        // Search both Professions and Services tables
        var professions =  await _context.Profession
            .Where(p => p.Name.Contains(search))
            .Select(p => new { Id = p.ProfessionId, Name = p.Name, Type = "Profession" })
            .ToListAsync();

        var ancillarys =  await _context.ProfessionAncillary
            .Where(s => s.Name.Contains(search))
            .Select(s => new { Id = s.AncillaryId, Name = s.Name, Type = "Ancillary" })
            .ToListAsync();

        // Combine the results from both tables
        var combinedResults = professions.Concat(ancillarys)
            .OrderBy(r => r.Type) // Optional: Order results by name
            .ToList();

        return Ok(combinedResults);
    }



        [HttpGet("searchprofessionsonly")]
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
    [Route("saveuserprofessions")]
    public IActionResult SaveUserProfessions([FromBody] UserProfessionsRequest request)
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

public class UserProfessionsRequest
{
    public int UserId { get; set; }
    public List<int> ProfessionIds { get; set; }
}



