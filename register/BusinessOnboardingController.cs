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
using System.Text.Json;


[ApiController]
[Route("api/[controller]")]
public class BusinessOnboardingController : ControllerBase
{
    private readonly AppDbContext _context;

    public BusinessOnboardingController(AppDbContext context)
    {
        _context = context;
    }



    [HttpPost]
    [Route("savebusinessservices")]
    public async Task<IActionResult> SaveBusinessServices([FromBody] BusinessServiceRequest request)
    {

        Console.WriteLine($"Arived at savebusinessservices");

        var existingServices = await _context.BusinessServices
            .Where(bp => bp.BusinessId == request.BusinessId)
            .ToListAsync();

        if (existingServices.Any())
        {
            _context.BusinessServices.RemoveRange(existingServices);
        }

        foreach (var serviceId in request.ServiceIds)
        {
            _context.BusinessServices.Add(new BusinessService
            {
                BusinessId = request.BusinessId,
                ServiceId = serviceId
            });
        }

        await _context.SaveChangesAsync();
        Console.WriteLine($"Business Service Details Saved ....");

        return Ok();
    }



    [HttpPost]
    [Route("savebusinessproducts")]
    public async Task<IActionResult> SaveBusinessProducts([FromBody] BusinessProductRequest request)
    {

        Console.WriteLine($"Arived at savebusinessproducts");

        var existingProducts = await _context.BusinessProducts
            .Where(bp => bp.BusinessId == request.BusinessId)
            .ToListAsync();

        if (existingProducts.Any())
        {
            _context.BusinessProducts.RemoveRange(existingProducts);
        }

        foreach (var productId in request.ProductIds)
        {
            _context.BusinessProducts.Add(new BusinessProducts
            {
                BusinessId = request.BusinessId,
                ProductId = productId
            });
        }

        await _context.SaveChangesAsync();
        Console.WriteLine($"Business Products Details Saved ....");

        return Ok();
    }




    [HttpPost("saveBusinessOnboarding01")]
    public async Task<IActionResult> SaveBusinessOnboarding01([FromBody] BusinessOnboardingRequest request)
    {

        Console.WriteLine($"Arived at saveBusinessOnboarding01");

        var jsonString = JsonSerializer.Serialize(request);
        Console.WriteLine($"Request: {jsonString}");


        Console.WriteLine($"Business ID : {request.BusinessId}");

        if (request == null || string.IsNullOrEmpty(request.BusinessType))
        {
            return BadRequest("Invalid business type or missing data.");
        }

        //using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // Step 1: Check if the business already exists
            Console.WriteLine($"Before running.....");
            //var existingBusiness = await _context.Businesses.FindAsync(request.BusinessId); // Find business by BusinessId (in this case, 1)
            
            int bid = request.BusinessId;
            Console.WriteLine($"BID = {bid}");
            var existingBusiness = await _context.Businesses
                .Where(b => b.BusinessId == bid)
                .FirstOrDefaultAsync(); 
            
            Console.WriteLine($"Afer running.....");




            if (existingBusiness != null)
            {
                Console.WriteLine($"Existing business found ");
                // Step 2: If the business exists, update its fields
                existingBusiness.BusinessType = request.BusinessType;
                existingBusiness.Residential = request.Residential;
                existingBusiness.Commercial = request.Commercial;
                existingBusiness.Government = request.Government;

                _context.Businesses.Update(existingBusiness); // This will update the existing business
            }
            else
            {
                Console.WriteLine($"Existing business NOT found ");
                // Step 3: If the business doesn't exist, add a new business
                var newBusiness = new Business
                {
                    BusinessType = request.BusinessType,
                    Residential = request.Residential,
                    Commercial = request.Commercial,
                    Government = request.Government
                };

                _context.Businesses.Add(newBusiness); // Add the new business
            }

            // Step 4: Save changes to the database
            await _context.SaveChangesAsync();
            Console.WriteLine($"Business Details Saved ....");
            

            // ********************************* PROFESSION IDS and ANCILLARY IDS

        
            var existingProfessions = await _context.BusinessProfessions
                .Where(bp => bp.BusinessId == request.BusinessId)
                .ToListAsync();

            if (existingProfessions.Any())
            {
                _context.BusinessProfessions.RemoveRange(existingProfessions);
            }

            // Step 2: Save selected professions into `business_profession`
            if (request.ProfessionIds != null && request.ProfessionIds.Any())
            {
                foreach (var professionId in request.ProfessionIds)
                {
                    var businessProfession = new BusinessProfession
                    {
                        BusinessId = 1, // Use the newly created business ID
                        ProfessionId = professionId
                    };

                    _context.BusinessProfessions.Add(businessProfession);
                }
                await _context.SaveChangesAsync();
                Console.WriteLine($"ProfessionIds  Saved ....");
            }


            // Step 4: Delete existing records for this business in `business_ancillary`
            var existingAncillaries = await _context.BusinessAncillaries
                .Where(ba => ba.BusinessId == request.BusinessId)
                .ToListAsync();

            if (existingAncillaries.Any())
            {
                _context.BusinessAncillaries.RemoveRange(existingAncillaries);
            }



            // Step 3: Save selected ancillaries into `business_ancillary`
            if (request.AncillaryIds != null && request.AncillaryIds.Any())
            {
                foreach (var ancillaryId in request.AncillaryIds)
                {
                    var businessAncillary = new BusinessAncillary
                    {
                        BusinessId = 1, // Use the newly created business ID
                        AncillaryId = ancillaryId
                    };

                    _context.BusinessAncillaries.Add(businessAncillary);
                }
                await _context.SaveChangesAsync();
                Console.WriteLine($"AncillaryIds  Saved ....");
            }

            

            //await transaction.CommitAsync();
            return Ok("Business onboarding saved successfully.");
        }
        catch (Exception ex)
        {
            //await transaction.RollbackAsync();
             Console.WriteLine($"ERROR.....{ex.Message.ToString()}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    // ************************************* CERTIFICATIONS **************************************

    [HttpGet("certifications")]
    public async Task<IActionResult> GetCertifications(long businessId)
    {
        // Step 1: Get the profession IDs associated with the business
        var professionIds = await _context.BusinessProfessions
            .Where(bp => bp.BusinessId == businessId)
            .Select(bp => bp.ProfessionId)
            .ToListAsync();

        if (professionIds == null || !professionIds.Any())
        {
            return NotFound("No professions found for the given business.");
        }

        // Step 2: Get the certifications based on the retrieved profession IDs
        var certifications = await _context.Certifications
            .Where(c => professionIds.Contains(c.ProfessionId))
            .ToListAsync();

        if (certifications == null || !certifications.Any())
        {
            return NotFound("No certifications found for the related professions.");
        }

        return Ok(certifications);
    }

    // Save accreditation details
    [HttpPost("saveaccreditation")]
    public async Task<IActionResult> SaveAccreditation([FromBody] BusinessCertificationRequest request)
    {

        Console.WriteLine($"Arrived in saveaccreditation....");

        // Update business details
        var business = await _context.Businesses.FindAsync(request.BusinessId);
        Console.WriteLine($"Arrived in saveaccreditation....");

        if (business != null)
        {
            business.LicenseNumber = request.LicenseNumber;
            business.LicenseExpiry = request.LicenseExpiry;
            business.FullyInsured = request.FullyInsured;

            _context.Businesses.Update(business);
        }
        else
        {
            return NotFound("Business not found");
        }

        // Remove existing certifications for this business
        var existingCertifications = await _context.BusinessCertifications
            .Where(bc => bc.BusinessId == request.BusinessId)
            .ToListAsync();

        _context.BusinessCertifications.RemoveRange(existingCertifications);

        // Add new certifications
        foreach (var certificationId in request.CertificationIds)
        {
            var businessCertification = new BusinessCertification
            {
                BusinessId = request.BusinessId,
                CertificationId = certificationId
            };
            _context.BusinessCertifications.Add(businessCertification);
        }

        await _context.SaveChangesAsync();
        return Ok("Accreditation details saved successfully.");
    }


    // ************************************* QUALIFICATIONS **************************************
    // Fetch Qualifications
    [HttpGet("qualifications")]
    public async Task<IActionResult> GetQualifications(long businessId)
    {
        var professionIds = await _context.BusinessProfessions
            .Where(bp => bp.BusinessId == businessId)
            .Select(bp => bp.ProfessionId)
            .ToListAsync();

        var qualifications = await _context.Qualifications
            .Where(q => professionIds.Contains(q.ProfessionId))
            .ToListAsync();

        return Ok(qualifications);
    }

    // Fetch Memberships
    [HttpGet("memberships")]
    public async Task<IActionResult> GetMemberships(long businessId)
    {
        var professionIds = await _context.BusinessProfessions
            .Where(bp => bp.BusinessId == businessId)
            .Select(bp => bp.ProfessionId)
            .ToListAsync();

        var memberships = await _context.IndustryMemberships
            .Where(m => professionIds.Contains(m.ProfessionId))
            .ToListAsync();

        return Ok(memberships);
    }


    // Save Qualifications and Memberships
    [HttpPost("saveQualificationsAndMemberships")]
    public async Task<IActionResult> SaveQualificationsAndMemberships([FromBody] BusinessQualificationMembershipRequest request)
    {
        var business = await _context.Businesses.FindAsync(request.BusinessId);
        if (business == null)
        {
            return NotFound("Business not found.");
        }

        // Update the business
        business.RegistrationNumber = request.RegistrationNumber ?? business.RegistrationNumber; // Only update if not null
        business.YearsExperience = request.YearsExperience ?? business.YearsExperience; // Only update if not null

        // Clear and update qualifications if provided
        if (request.QualificationIds != null)
        {
            var existingQualifications = await _context.BusinessQualifications
                .Where(bq => bq.BusinessId == request.BusinessId)
                .ToListAsync();
            _context.BusinessQualifications.RemoveRange(existingQualifications);

            foreach (var qualificationId in request.QualificationIds)
            {
                var businessQualification = new BusinessQualification
                {
                    BusinessId = request.BusinessId,
                    QualificationId = qualificationId
                };
                _context.BusinessQualifications.Add(businessQualification);
            }
        }


        if (request.MembershipIds != null)
            {
                var existingMemberships = await _context.BusinessMemberships
                    .Where(bm => bm.BusinessId == request.BusinessId)
                    .ToListAsync();
                _context.BusinessMemberships.RemoveRange(existingMemberships);

                foreach (var membershipId in request.MembershipIds)
                {
                    var businessMembership = new BusinessMembership
                    {
                        BusinessId = request.BusinessId,
                        MembershipId = membershipId

                    };
                    _context.BusinessMemberships.Add(businessMembership);
                }
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            return Ok("Qualifications and memberships saved successfully.");

    }


}

