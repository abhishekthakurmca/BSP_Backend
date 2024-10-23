using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using MyBackendApp.Models.BusinessModels;
using MyBackendApp.Models;
using MyBackendApp.Data;

namespace MyBackendApp.Controllers.Businesses
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusinessProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BusinessProfileController> _logger;
        public BusinessProfileController(ApplicationDbContext context, ILogger<BusinessProfileController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("getBusinessProfile")]
        public async Task<ActionResult<Business>> GetBusinessProfile(int businessId)
        {
            var business = await _context.Businesses
                .Where(b => b.BusinessId == businessId)
                .Select(b => new Business
                {
                    BusinessId = b.BusinessId,
                    BusinessName = b.BusinessName ?? string.Empty,
                    Phone = b.Phone ?? string.Empty,
                    Email = b.Email ?? string.Empty,
                    StreetNumber = b.StreetNumber ?? string.Empty,
                    StreetName = b.StreetName ?? string.Empty,
                    Suburb = b.Suburb ?? string.Empty,
                    PostCode = b.PostCode ?? string.Empty,
                    MyState = b.MyState ?? string.Empty,
                    Overview = b.Overview ?? string.Empty,
                    Description = b.Description ?? string.Empty,
                    OverallRating = b.OverallRating ?? 0,

                    LicenseNumber = b.LicenseNumber ?? string.Empty,
                    FormattedLicenseExpiry = b.LicenseExpiry.HasValue ? b.LicenseExpiry.Value.ToString("yyyy-MM-dd") : string.Empty,

                    RegistrationNumber = b.RegistrationNumber ?? string.Empty,
                    YearsExperience = b.YearsExperience ?? 0,
                    FullyInsured = b.FullyInsured ?? false,

                    Residential = b.Residential ?? 0,
                    Commercial = b.Commercial ?? 0,
                    Government = b.Government ?? 0,

                    AfterHours = b.AfterHours ?? 0,
                    Weekends = b.Weekends ?? 0,

                })
                .FirstOrDefaultAsync();

            if (business == null)
            {
                return NotFound();
            }
            _logger.LogInformation("Business fetched: {business}",JsonSerializer.Serialize(business));

            var query = "SELECT c.certificationid, c.name FROM business_certification bc JOIN certification c ON bc.certificationid = c.certificationid WHERE bc.businessid = @businessId";
            var certifications = await _context.Certifications.FromSqlRaw(query, new MySqlConnector.MySqlParameter("@businessId", businessId)).Select(c => new Certification
            {
                CertificationId = c.CertificationId,
                Name = c.Name ?? string.Empty
            }).ToListAsync();

            business.certifications = certifications;

            var reviews = await (from r in _context.ReviewOfBusiness
                                 where r.BusinessId == businessId
                                 select new ReviewOfBusiness
                                 {
                                     ReviewId = r.ReviewId,
                                     BusinessId = r.BusinessId,
                                     JobId = r.JobId,
                                     DateAdded = r.DateAdded,
                                     Description = r.Description ?? string.Empty,
                                     OverallRating = r.OverallRating
                                 }).ToListAsync();


            var querySql = @"SELECT s.service_id, s.name, s.description 
                                 FROM business_service bs
                                 JOIN services s ON bs.serviceid = s.service_id
                                 WHERE bs.businessid = @businessId";
            var services = await _context.Services.FromSqlRaw(querySql, new MySqlConnector.MySqlParameter("@businessId", businessId)).Select(c => new Service
            {
                ServiceId = c.ServiceId,
                Name = c.Name ?? string.Empty,
                Description = c.Description ?? string.Empty

            }).ToListAsync();

            var result = new
            {
                Businesses = business,
                Reviews = reviews,
                Certifications = certifications,
                Services = services
            };
            return Ok(result);
        }
    }
}


