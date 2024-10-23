using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models.HomeProfile;

[Table("industry")]
public class WorkingIndustry
{
    [Key]
    [Column("industry_id")]
    public int IndustryId { get; set; }
    public string Name { get; set; }
}