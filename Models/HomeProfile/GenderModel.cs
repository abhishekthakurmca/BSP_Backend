using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models.HomeProfile;

[Table("gender")]
public class GenderModel
{
    [Key]
    public int Id { get; set; }
    public string Gender { get; set; }
}