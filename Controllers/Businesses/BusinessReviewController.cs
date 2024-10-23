using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBackendApp.Data;
using MyBackendApp.Models.BusinessModels;

namespace MyBackendApp.Controllers.Businesses;


[ApiController]
[Route("api/[controller]")]
public class BusinessReviewController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<BusinessReviewController> _logger;
    public BusinessReviewController(ApplicationDbContext context, Logger<BusinessReviewController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost("save")]
    public async Task<IActionResult> SaveReview([FromBody] ReviewOfBusiness review)
    {
        _logger.LogInformation("Business fetched: {review}", JsonSerializer.Serialize(review));

        review.MyStatus = "draft";
        review.DateAdded = DateTime.UtcNow; // Set the dateAdded when saving the draft
        review.DateModified = DateTime.UtcNow;

        _context.ReviewOfBusiness.Add(review);
        await _context.SaveChangesAsync();

        return Ok(new { reviewId = review.ReviewId, message = "Review saved as draft" });
    }

    [HttpPost("publish")]
    public async Task<IActionResult> PublishReview([FromBody] ReviewOfBusiness review)
    {
        review.MyStatus = "published";
        review.DateModified = DateTime.UtcNow; // Set the dateModified when publishing

        // If the review already exists, update it. Otherwise, add a new one.
        var existingReview = await _context.ReviewOfBusiness.FirstOrDefaultAsync(r => r.ReviewId == review.ReviewId);
        if (existingReview != null)
        {
            existingReview.MyStatus = review.MyStatus;
            existingReview.Description = review.Description;
            existingReview.OverallRating = review.OverallRating;
            existingReview.RatingQuality = review.RatingQuality;
            existingReview.RatingService = review.RatingService;
            existingReview.RatingCommunication = review.RatingCommunication;
            existingReview.RatingPromptness = review.RatingPromptness;
            existingReview.RatingCompletion = review.RatingCompletion;
            existingReview.DateModified = DateTime.UtcNow;
        }
        else
        {
            review.DateAdded = DateTime.UtcNow;
            _context.ReviewOfBusiness.Add(review);
        }
        await _context.SaveChangesAsync();
        return Ok("Review published");
    }
}
