using System;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models.BusinessModels;

    public class ReviewOfBusiness
    {
        [Key]
        [Column("reviewid")]
        public long ReviewId { get; set; }
        public long BusinessId { get; set; }
        public long? JobId { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        public DateTime DateModified { get; set; } = DateTime.UtcNow;

        public string BusinessName { get; set; }
        public string Description { get; set; }
        public string MyStatus { get; set; }
        public decimal? OverallRating { get; set; }
        public long? UserId { get; set; }
        public long? MediaId { get; set; }
        public int? RatingQuality { get; set; }
        public int? RatingService { get; set; }
        public int? RatingCommunication { get; set; }
        public int? RatingPromptness { get; set; }
        public int? RatingCompletion { get; set; }
    }
