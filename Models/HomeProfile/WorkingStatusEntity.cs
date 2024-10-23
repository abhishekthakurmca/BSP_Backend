
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models.HomeProfile;

[Table("working_status")]
public class WorkingStatusEntity
{
    [Key]
    public int Id { get; set; }
    public string WorkingStatus { get; set; }
}