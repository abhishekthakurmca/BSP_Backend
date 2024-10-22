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
using Microsoft.EntityFrameworkCore;

using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Asn1.Iana;

using MyBackendApp.search.models;


[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly AppDbContext _context;

    public SearchController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("searchall")]
    public async Task<IActionResult> GetAllStypes(string search) 
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

                    // Search both Professions and Services tables
        var professionsancillary =  await _context.ProfessionAncillary
            .Where(p => p.Name.Contains(search))
            .Select(p => new { Id = p.AncillaryId, Name = p.Name, Type = "ProfessionAncillary" })
            .ToListAsync();

        var suppliertypes =  await _context.SupplierType
            .Where(s => s.Name.Contains(search))
            .Select(s => new { Id = s.SupplierTypeId, Name = s.Name, Type = "SupplierType" })
            .ToListAsync();

        // Combine the results from both tables
        var combinedResults = professions
            .Concat(services)
            .Concat(professionsancillary)
            .Concat(suppliertypes)
            .OrderBy(r => r.Type) // Optional: Order by name or other criteria
            .ToList();

        return Ok(combinedResults);
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


    [HttpGet("searchservices")]
    public async Task<IActionResult> GetServices(string search)
    {
        if (string.IsNullOrEmpty(search)) return BadRequest("Search term is required.");

        var services = await _context.Services
            .Where(p => p.Name.Contains(search))
            .Select(s => new { Id = s.ServiceId, Name = s.Name, Type = "Service" })
            .ToListAsync();

        return Ok(services);
    }

    [HttpGet("searchproducts")]
    public async Task<IActionResult> GetProducts(string search)
    {
        if (string.IsNullOrEmpty(search)) return BadRequest("Search term is required.");

        var products = await _context.Products
            .Where(p => p.ProductName.Contains(search))
            .Select(s => new { Id = s.ProductId, Name = s.ProductName, Type = "Product" })
            .ToListAsync();

        return Ok(products);
    }

    // ******************************** POSTCODE SEARCH *******************************

    
    [HttpPost("searchprofpostcode")]
    public async Task<IActionResult> SearchBusinesses([FromBody] SearchRequest request)
    {

        Console.WriteLine("arrived at searchprofpostcode...");

        Console.WriteLine($"request.SearchTerm...{request.SearchTerm}");
        Console.WriteLine($"request.PostCode...{request.PostCode}");
        

        // Search the professions table for matching professions
        var professions = await _context.Profession
            .Where(p => p.Name.Contains(request.SearchTerm) || p.Description.Contains(request.SearchTerm))
            .Select(p => p.ProfessionId)
            .ToListAsync();

        if (!professions.Any())
        {
            return NotFound("No matching professions found");
        }

        // Get certifications for all matching professionIds
        var certs = await _context.Certifications
            .Where(c => professions.Contains(c.ProfessionId))
            .ToListAsync();

        if (!certs.Any())
        {
            return NotFound("No certifications found for the given professions");
        }





        // Find matching businesses by profession
        var businessIds = await _context.BusinessProfessions
            .Where(bp => professions.Contains(bp.ProfessionId))
            .Select(bp => bp.BusinessId)
            .Distinct()
            .ToListAsync();

        // Get the user's postcode details
        var originPostcode = await _context.OzPostcodes
            .FirstOrDefaultAsync(p => p.Postcode == request.PostCode);

        if (originPostcode == null)
        {
            return NotFound("Postcode not found");
        }

        // Find all postcodes within a 25km radius
        var allPostcodes = await _context.OzPostcodes
            .Where(p => p.Mystate == originPostcode.Mystate)
            .ToListAsync();

        var nearbyPostcodes = FindLocationsWithinRadius(allPostcodes, originPostcode.Latitude, originPostcode.Longitude, 25);

        var businesses = await _context.Businesses
            .Where(b => businessIds.Contains(b.BusinessId) && nearbyPostcodes.Select(p => p.Postcode).Contains(b.PostCode))
            .ToListAsync();


        // get certification data for this professionid


        var result = new
        {
            Businesses = businesses,
            Locations = businesses.Select(b => new { b.Latitude, b.Longitude, b.BusinessName }).ToList(),
            Center = new { Latitude = originPostcode.Latitude, Longitude = originPostcode.Longitude },
            Professions = professions,
            Certifications = certs,
        };

        // bypass results with locations etc
        return Ok(result);
    }



    [HttpPost("searchprofpostcodelist")]
    public async Task<IActionResult> SearchBusinessesList([FromBody] SearchRequest request)
    {

        Console.WriteLine("arrived at searchprofpostcode...");

        Console.WriteLine($"request.SearchTerm...{request.SearchTerm}");
        Console.WriteLine($"request.PostCode...{request.PostCode}");
        

        // Search the professions table for matching professions
        var professions = await _context.Profession
            .Where(p => p.Name.Contains(request.SearchTerm) || p.Description.Contains(request.SearchTerm))
            .Select(p => p.ProfessionId)
            .ToListAsync();

        if (!professions.Any())
        {
            return NotFound("No matching professions found");
        }

        // Find matching businesses by profession
        var businessIds = await _context.BusinessProfessions
            .Where(bp => professions.Contains(bp.ProfessionId))
            .Select(bp => bp.BusinessId)
            .Distinct()
            .ToListAsync();

        // Get the user's postcode details
        var originPostcode = await _context.OzPostcodes
            .FirstOrDefaultAsync(p => p.Postcode == request.PostCode);

        if (originPostcode == null)
        {
            return NotFound("Postcode not found");
        }

        // Find all postcodes within a 25km radius
        var allPostcodes = await _context.OzPostcodes
            .Where(p => p.Mystate == originPostcode.Mystate)
            .ToListAsync();

        var nearbyPostcodes = FindLocationsWithinRadius(allPostcodes, originPostcode.Latitude, originPostcode.Longitude, 25);

        var businesses = await _context.Businesses
            .Where(b => businessIds.Contains(b.BusinessId) && nearbyPostcodes.Select(p => p.Postcode).Contains(b.PostCode))
            .ToListAsync();



        // Iterate over the businesses and print their details
        foreach (var business in businesses)
        {
            Console.WriteLine($"Business ID: {business.BusinessId}, Name: {business.BusinessName}, Postcode: {business.PostCode}");
        }

/*
        var result = new
        {
            Businesses = businesses,
            Locations = businesses.Select(b => new { b.Latitude, b.Longitude, b.BusinessName }).ToList(),
            Center = new { Latitude = originPostcode.Latitude, Longitude = originPostcode.Longitude }
        };
*/
        // bypass results with locations etc
        return Ok(businesses);
    }




    // This method uses the Haversine formula for calculating distance between two points
    private List<OzPostcode> FindLocationsWithinRadius(List<OzPostcode> postcodes, decimal originLat, decimal originLng, int? radiusKm)
    {
        return postcodes.Where(p => 
            GetDistanceFromLatLonInKm(originLat, originLng, p.Latitude, p.Longitude) <= radiusKm).ToList();
    }

    private double GetDistanceFromLatLonInKm(decimal lat1, decimal lon1, decimal lat2, decimal lon2)
    {
        const int R = 6371; // Radius of the Earth in km
        var dLat = (double)(lat2 - lat1) * (Math.PI / 180);
        var dLon = (double)(lon2 - lon1) * (Math.PI / 180);
        var a =
            Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
            Math.Cos((double)lat1 * (Math.PI / 180)) * Math.Cos((double)lat2 * (Math.PI / 180)) *
            Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c; // Distance in km
    }


[HttpPost("searchwithfilters")]
public async Task<IActionResult> SearchBusinessesWithFilters([FromBody] SearchRequest request)
{
    // Step 1: Search the professions table for matching professions (as before)
    var professions = await _context.Profession
        .Where(p => p.Name.Contains(request.SearchTerm) || p.Description.Contains(request.SearchTerm))
        .Select(p => p.ProfessionId)
        .ToListAsync();

    if (!professions.Any())
    {
        return NotFound("No matching professions found");
    }

    // Step 2: Find matching businesses by profession
    var businessIds = await _context.BusinessProfessions
        .Where(bp => professions.Contains(bp.ProfessionId))
        .Select(bp => bp.BusinessId)
        .Distinct()
        .ToListAsync();

    // Step 3: Get the user's postcode details
    var originPostcode = await _context.OzPostcodes
        .FirstOrDefaultAsync(p => p.Postcode == request.PostCode);

    if (originPostcode == null)
    {
        return NotFound("Postcode not found");
    }

    // Step 4: Find all postcodes within a certain radius (default 25km or custom)
    var allPostcodes = await _context.OzPostcodes
        .Where(p => p.Mystate == originPostcode.Mystate)
        .ToListAsync();

    var nearbyPostcodes = FindLocationsWithinRadius(allPostcodes, originPostcode.Latitude, originPostcode.Longitude, request.Within);

    // Step 5: Get the businesses within the profession and nearby postcodes
    var businessesQuery = _context.Businesses
        .Where(b => businessIds.Contains(b.BusinessId) && nearbyPostcodes.Select(p => p.Postcode).Contains(b.PostCode));


    // Logging or printing values of request.WorkScope
    if (request.WorkScope != null)
    {
        Console.WriteLine($"WorkScope.Residential: {request.WorkScope.Residential}");
        Console.WriteLine($"WorkScope.Commercial: {request.WorkScope.Commercial}");
        Console.WriteLine($"WorkScope.Government: {request.WorkScope.Government}");
    }
    else
    {
        Console.WriteLine("WorkScope is null");
    }


    // Step 6: Apply work scope filters (AND logic)
    if (request.WorkScope != null)
    {
        if (request.WorkScope.Residential && request.WorkScope.Commercial && request.WorkScope.Government)
        {
            // do nothing
        }
        else 
        {
            /*
            businessesQuery = businessesQuery.Where(b => 
                (b.Residential == (request.WorkScope.Residential ? 1 : 0)) && 
                (b.Commercial == (request.WorkScope.Commercial ? 1 : 0)) && 
                (b.Government == (request.WorkScope.Government ? 1 : 0)));
            */
            if (request.WorkScope != null)
            {
                if (request.WorkScope.Residential)
                {
                    businessesQuery = businessesQuery.Where(b => b.Residential == 1);  // Treat 1 as true
                }
                if (request.WorkScope.Commercial)
                {
                    businessesQuery = businessesQuery.Where(b => b.Commercial == 1);  // Treat 1 as true
                }
                if (request.WorkScope.Government)
                {
                    businessesQuery = businessesQuery.Where(b => b.Government == 1);  // Treat 1 as true
                }
            }
        }
    }

        // After hours and weekends
        if (request.Options != null)
        {
            Console.WriteLine($"AfterHours: {request.Options.AfterHours}");
            Console.WriteLine($"Weekends: {request.Options.Weekends}");

            int newhours = Convert.ToInt32(request.Options.AfterHours);
            Console.WriteLine($"newhours: {newhours}");
            int newweekend = Convert.ToInt32(request.Options.Weekends);

            if (newhours == 0 && newweekend == 0)
            {
                // do nothing
            }
            else {
                    if (request.Options.AfterHours)
                    {
                        businessesQuery = businessesQuery.Where(b => b.AfterHours == newhours);  // Treat 1 as true
                    }
                    if (request.Options.Weekends)
                    {
                        businessesQuery = businessesQuery.Where(b => b.Weekends == newweekend);  // Treat 1 as true
                    }
            }
        }


    // Rating - filters
    if (request.Ratings != null) {

        Console.WriteLine($"Overall Rating: {request.Ratings}");

        string ratingString = request.Ratings;

        float newrating = (float)char.GetNumericValue(ratingString[0]);

        Console.WriteLine(newrating);

        // Convert the float (newrating) to decimal
        decimal ratingDecimal = Convert.ToDecimal(newrating);

        // Perform the comparison, checking if OverallRating is not null before comparing
        businessesQuery = businessesQuery.Where(b => b.OverallRating != null && b.OverallRating >= ratingDecimal);

    }



    // Execute the query and bring results to memory
    var businesses = await businessesQuery.ToListAsync(); // Move to memory


    Console.WriteLine($"Keyword: {request.KeywordSearch}");

    // Step 7: Apply additional keyword search on business.overview and business.description in memory
    if (!string.IsNullOrEmpty(request.KeywordSearch))
    {
        var keywords = request.KeywordSearch.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        if (keywords.Any())
        {
            businesses = businesses
                .Where(b =>
                    keywords.Any(k =>
                        (!string.IsNullOrEmpty(b.Overview) && b.Overview.Contains(k)) ||
                        (!string.IsNullOrEmpty(b.Description) && b.Description.Contains(k))
                    ))
                .ToList(); // Apply filtering in memory
        }
    }

    // Step 8: Prepare the result
    var result = new
    {
        Businesses = businesses,
        Locations = businesses.Select(b => new { b.Latitude, b.Longitude, b.BusinessName }).ToList(),
        Center = new { Latitude = originPostcode.Latitude, Longitude = originPostcode.Longitude }
    };

    return Ok(result);
}


}
