using Microsoft.AspNetCore.Mvc;
using MyBackendApp.Models.HomeProfile;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using MyBackendApp.Data;
using MyBackendApp.Dto.Construction;
using MyBackendApp.Dto.PersonalInterest;

namespace MyBackendApp.Controllers.Home;
[ApiController]
[Route("api/[controller]")]
public class HomeOnboardingController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<HomeOnboardingController> _logger;
    public HomeOnboardingController(ApplicationDbContext context, Logger<HomeOnboardingController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("dropdownData")]
    public async Task<IActionResult> GetDropdownData()
    {
        var genders = await _context.Genders.ToListAsync();
        var ageGroups = await _context.AgeGroups.ToListAsync();
        var workingStatuses = await _context.WorkingStatuses.ToListAsync();
        var livingStatuses = await _context.LivingStatuses.ToListAsync();
        var industries = await _context.Industries.ToListAsync();

        return Ok(new { genders, ageGroups, workingStatuses, livingStatuses, industries });
    }

    [HttpPut("updateProfile01")]
    public async Task<IActionResult> UpdateProfile([FromBody] HomeUserProfileModel profile)
    {
        _logger.LogInformation("Arrived at update-profile");

        try
        {
            _logger.LogInformation("about to _context.HomeUserProfile");

            var existingProfile = await _context.HomeUserProfile
                .FirstOrDefaultAsync(p => p.UserId == profile.UserId);
            if (existingProfile == null)
            {

                _logger.LogInformation($"NULL Profile");
                return NotFound("Profile not found");

            }

            _logger.LogInformation("processing existingProfile - {workingIndustry}", profile.WorkingIndustry);
            existingProfile.AgeGroup = profile.AgeGroup;
            existingProfile.WorkingStatus = profile.WorkingStatus;
            existingProfile.LivingStatus = profile.LivingStatus;
            existingProfile.WorkingIndustry = profile.WorkingIndustry;

            _context.HomeUserProfile.Update(existingProfile);
            await _context.SaveChangesAsync();

            return Ok(existingProfile);

        }
        catch (Exception ex)
        {
            _logger.LogInformation($"Error: {ex.Message.ToString()}");
            return StatusCode(500, "Internal server error");
        }

    }


    [HttpPost("saveSearchSelections")]
    public async Task<IActionResult> SaveSelections([FromBody] SaveSelectionsRequest request)
    {

        var jsonString = JsonSerializer.Serialize(request);
        _logger.LogInformation("Request : {request}", jsonString);

        try
        {
            // Save professions
            if (request.ProfessionIds != null && request.ProfessionIds.Any())
            {

                // Check if the user already has selected professions
                var existingProfessions = await _context.HomeUserLookingForProfessions
                    .Where(p => p.UserId == request.UserId)
                    .ToListAsync();

                // Remove existing professions before adding new ones
                _context.HomeUserLookingForProfessions.RemoveRange(existingProfessions);

                foreach (var professionId in request.ProfessionIds)
                {
                    var profession = new HomeUserLookingForProfession
                    {
                        UserId = request.UserId,
                        ProfessionId = professionId
                    };
                    _context.HomeUserLookingForProfessions.Add(profession);
                }
            }

            // Save services
            if (request.ServiceIds != null && request.ServiceIds.Any())
            {

                // Check if the user already has selected professions
                var existingServices = await _context.HomeUserLookingForServices
                    .Where(p => p.UserId == request.UserId)
                    .ToListAsync();

                // Remove existing professions before adding new ones
                _context.HomeUserLookingForServices.RemoveRange(existingServices);

                foreach (var serviceId in request.ServiceIds)
                {
                    var service = new HomeUserLookingForServices
                    {
                        UserId = request.UserId,
                        ServiceId = serviceId
                    };
                    _context.HomeUserLookingForServices.Add(service);
                }
            }

            // Save supplier types
            if (request.SupplierTypeIds != null && request.SupplierTypeIds.Any())
            {

                // Check if the user already has selected professions
                var existingSuppliers = await _context.HomeUserLookingForSuppliers
                    .Where(p => p.UserId == request.UserId)
                    .ToListAsync();

                // Remove existing professions before adding new ones
                _context.HomeUserLookingForSuppliers.RemoveRange(existingSuppliers);

                foreach (var supplierTypeId in request.SupplierTypeIds)
                {
                    var supplier = new HomeUserLookingForSupplier
                    {
                        UserId = request.UserId,
                        SupplierTypeId = supplierTypeId
                    };
                    _context.HomeUserLookingForSuppliers.Add(supplier);
                }
            }

            // Save profession ancillaries
            if (request.AncillaryIds != null && request.AncillaryIds.Any())
            {

                // Check if the user already has selected professions
                var existingPA = await _context.HomeUserLookingForProfessionAncillaries
                    .Where(p => p.UserId == request.UserId)
                    .ToListAsync();

                // Remove existing professions before adding new ones
                _context.HomeUserLookingForProfessionAncillaries.RemoveRange(existingPA);

                foreach (var ancillaryId in request.AncillaryIds)
                {
                    var ancillary = new HomeUserLookingForProfessionAncillary
                    {
                        UserId = request.UserId,
                        AncillaryId = ancillaryId
                    };
                    _context.HomeUserLookingForProfessionAncillaries.Add(ancillary);
                }
            }

            // Save changes to the database
            //await _context.SaveChangesAsync();
            //return Ok("Selections saved successfully");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Add the profession, service, supplier, and ancillary selections here...

                // Save changes
                await _context.SaveChangesAsync();

                // Commit transaction
                await transaction.CommitAsync();
                return Ok("Selections saved successfully");
            }
            catch (Exception ex)
            {
                // Rollback transaction if any error occurs
                await transaction.RollbackAsync();
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }



        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    // ************************* INTERESTS ******************************

    [HttpGet("constructionInterests")]
    public async Task<IActionResult> GetConstructionInterests()
    {
        var cInterests = await _context.ConstructionInterests.ToListAsync();

        _logger.LogInformation("Fetched Construction Interests:", cInterests);

        return Ok(cInterests);
    }

    [HttpGet("personalInterests")]
    public async Task<IActionResult> GetPersonalInterests()
    {
        var pInterests = await _context.PersonalInterests.ToListAsync();
        return Ok(pInterests);
    }

    [HttpPost("saveConstructionInterests")]
    public async Task<IActionResult> SaveConstructionInterests([FromBody] ConstructionInterestRequestDto request)
    {

        _logger.LogInformation("Arrived at saveConstructionInterests");
        var jsonString = JsonSerializer.Serialize(request);
        _logger.LogInformation("Request:", jsonString);


        if (request.CInterestIds == null || !request.CInterestIds.Any())
        {
            return BadRequest("No construction interests selected.");
        }

        try
        {

            // Step 1: Find existing construction interests for the user
            var existingInterests = await _context.HomeUserConstructionInterests
                .Where(ci => ci.UserId == request.UserId)
                .ToListAsync();

            // Step 2: Remove existing interests
            _context.HomeUserConstructionInterests.RemoveRange(existingInterests);

            foreach (var cInterestId in request.CInterestIds)
            {
                var userConstructionInterest = new HomeUserConstructionInterest
                {
                    UserId = request.UserId,
                    CInterestId = cInterestId
                };
                _context.HomeUserConstructionInterests.Add(userConstructionInterest);
            }

            await _context.SaveChangesAsync();
            return Ok("Construction interests saved successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("savePersonalInterests")]
    public async Task<IActionResult> SavePersonalInterests([FromBody] PersonalInterestRequestDto request)
    {
        if (request.PInterestIds == null || !request.PInterestIds.Any())
        {
            return BadRequest("No personal interests selected.");
        }

        try
        {
            var existingInterests = await _context.HomeUserPersonalInterests
                .Where(ci => ci.UserId == request.UserId)
                .ToListAsync();
            _context.HomeUserPersonalInterests.RemoveRange(existingInterests);


            foreach (var pInterestId in request.PInterestIds)
            {
                var userPersonalInterest = new HomeUserPersonalInterest
                {
                    UserId = request.UserId,
                    PInterestId = pInterestId
                };
                _context.HomeUserPersonalInterests.Add(userPersonalInterest);
            }

            await _context.SaveChangesAsync();
            return Ok("Personal interests saved successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
