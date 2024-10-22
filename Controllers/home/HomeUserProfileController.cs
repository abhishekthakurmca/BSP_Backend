
using Microsoft.AspNetCore.Mvc;

using MyBackendApp.Models;
using Microsoft.EntityFrameworkCore;

using MyBackendApp.Models.Jobs;
using System.Text.Json;
using MyBackendApp.Models.HomeProfile;
using MyBackendApp.Models.BusinessNameSpace;
using MyBackendApp.Models.BusinessModels;

namespace MyBackendApp.Controllers
{
[ApiController]
[Route("api/HomeProfile")]
public class HomeuserProfileController : ControllerBase
{
    private readonly AppDbContext _context;

    public HomeuserProfileController(AppDbContext context)
    {
        _context = context;
    }

        [HttpGet("getuserprofile")]
        public async Task<ActionResult<HomeUserModel>> GetHomeUserProfile([FromQuery] int userId)
        {
            var user = await _context.HomeUsers
                .Where(u => u.user_id == userId)
                .Select(u => new HomeUserModel
                {
                    user_id = u.user_id,
                    Email = u.Email ?? string.Empty,
                    Phone = u.Phone ?? string.Empty,
                    FirstName = u.FirstName ?? string.Empty,
                    LastName = u.LastName ?? string.Empty,
                    Postcode = u.Postcode ?? string.Empty,
                    Suburb = u.Suburb ?? string.Empty,
                })
                .FirstOrDefaultAsync();

            Console.WriteLine($"User fetched: {JsonSerializer.Serialize(user)}");
            //System.Diagnostics.Debug.WriteLine($"User fetched: {user}");

            if (user == null)
            {
                return NotFound();
            }

            var jobs = await _context.Jobs
                .Where(j => j.user_id == userId)
                .Select(j => new JobModel
                {
                    job_id = j.job_id,
                    user_id = j.user_id,
                    name = j.name ?? string.Empty,
                    description = j.description ?? string.Empty,
                    budget = j.budget,
                    eta = j.eta,
                    postcode = j.postcode ?? string.Empty,
                    job_status = j.job_status ?? string.Empty,
                    assigned_to = j.assigned_to,
                    business_name = _context.Businesses
                    .Where(b => b.BusinessId == j.assigned_to)
                    .Select(b => b.BusinessName) 
                    .FirstOrDefault()
                })
                .ToListAsync();

            // Print to debug console
            System.Diagnostics.Debug.WriteLine($"Jobs fetched: {jobs.Count}");

            var interests = await (from hci in _context.HomeUserConstructionInterests
                                   join ci in _context.ConstructionInterests on hci.CInterestId equals ci.CInterestId
                                   where hci.UserId == userId
                                   select new ConstructionInterest
                                   {
                                       CInterestId = ci.CInterestId,
                                       InterestName = ci.InterestName ?? string.Empty
                                   }).ToListAsync();

            // Print to debug console
            System.Diagnostics.Debug.WriteLine($"Interests fetched: {interests.Count}");


            var reviews = await (from r in _context.ReviewOfBusiness
                                 join b in _context.Businesses on r.BusinessId equals b.BusinessId
                                 where r.UserId == userId
                                 select new ReviewOfBusiness
                                 {
                                     ReviewId = r.ReviewId,
                                     BusinessId = r.BusinessId,
                                     BusinessName = b.BusinessName ?? string.Empty,
                                     JobId = r.JobId,
                                     DateAdded = r.DateAdded,
                                     Description = r.Description ?? string.Empty,
                                     OverallRating = r.OverallRating
                                 }).ToListAsync();

            // Print to debug console
            System.Diagnostics.Debug.WriteLine($"Reviews fetched: {reviews.Count}");



            var userProfile = new CustomerProfileModel
            {
                User = user,
                Jobs = jobs,
                ConstructionInterests = interests,
                BusinessReviews = reviews
            };

            return Ok(userProfile);
        }


    [HttpGet("searchprofessions")]
    public async Task<IActionResult> GetProfessions(string search)
    {
        if (string.IsNullOrEmpty(search)) return BadRequest("Search term is required.");

        var professions = await _context.Profession
            .Where(p => p.Name.Contains(search))
            .Select(p => new { p.ProfessionId, p.Name })
            .ToListAsync();

        return Ok(professions);
    }


}


}
