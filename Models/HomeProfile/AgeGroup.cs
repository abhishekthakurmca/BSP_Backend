using System.ComponentModel.DataAnnotations;

namespace MyBackendApp.Models.HomeProfile;

public class AgeGroup
{
    [Key]
    public int Id { get; set; }
    public string Age { get; set; }
}