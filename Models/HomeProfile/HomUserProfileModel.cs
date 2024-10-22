
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models.HomeProfile
{
    [Table("homeuser_profile")]
    public class HomeUserProfileModel
    {
    [Key]
    [Column("profile_id")]
    public long ProfileId { get; set; }
    [Column("user_id")]
    public long UserId { get; set; }
    //public string? Description { get; set; }

    [Column("age_group")]
    public string? AgeGroup { get; set; }

    [Column("working_status")]
    public string? WorkingStatus { get; set; }

    [Column("living_status")]
    public string? LivingStatus { get; set; }

    [Column("working_industry")]
    public string? WorkingIndustry { get; set; }

    }
}