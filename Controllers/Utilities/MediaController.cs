using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBackendApp.Data;
using MyBackendApp.Models.BusinessNameSpace;

namespace MyBackendApp.Controllers.Utilities
{
    [ApiController]
    [Route("api/[controller]")]
    public class MediaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public MediaController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpPost("uploadImage")]
        public async Task<IActionResult> UploadMedia([FromForm] IFormCollection form)
        {
            Console.WriteLine($"Arrived in upload:");

            var businessId = long.Parse(form["businessId"]);
            var userType = form["userType"];
            var files = form.Files;

            if (files.Count == 0)
            {
                return BadRequest("No files received.");
            }

            var uploadedFiles = new List<MediaForBusiness>();

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    //var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "business", userType, businessId.ToString());
                    var uploadsFolder = @"C:\inetpub\vhosts\newintestserver.xyz\app.newintestserver.xyz\wwwroot\uploads\business\reviews\1";

                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var media = new MediaForBusiness
                    {
                        BusinessId = businessId,
                        DateCreated = DateTime.UtcNow,
                        MediaType = Path.GetExtension(file.FileName),
                        MediaUrl = $"/uploads/{userType}/{businessId}/{uniqueFileName}"
                    };

                    _context.MediaForBusiness.Add(media);
                    uploadedFiles.Add(media);
                }
            }

            await _context.SaveChangesAsync();

            return Ok(uploadedFiles);
        }

        [HttpGet("media/{businessId}")]
        public async Task<IActionResult> GetMedia(long businessId)
        {
            var media = await _context.MediaForBusiness
                .Where(m => m.BusinessId == businessId)
                .ToListAsync();

            var baseUrl = $"{Request.Scheme}://{Request.Host}/uploads/";
            media.ForEach(m => m.MediaUrl = $"{baseUrl}{m.MediaUrl}");

            Console.WriteLine($"media fetched: {string.Join(", ", media.Select(m => m.MediaUrl))}");

            return Ok(media);
        }



        [HttpGet("mediaFile/{businessId}/{fileName}")]
        public IActionResult GetMediaFile(long businessId, string fileName)
        {
            try
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "business", "reviews", businessId.ToString());
                var filePath = Path.Combine(uploadsFolder, fileName);

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound("File not found.");
                }

                var contentType = "application/octet-stream";
                var extension = Path.GetExtension(filePath).ToLowerInvariant();
                switch (extension)
                {
                    case ".jpg":
                    case ".jpeg":
                        contentType = "image/jpeg";
                        break;
                    case ".png":
                        contentType = "image/png";
                        break;
                    case ".pdf":
                        contentType = "application/pdf";
                        break;
                }
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, contentType, fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}