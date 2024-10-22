
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models.HomeProfile
{
    [Table("living_status")]
    public class LivingStatusModel
    {
    [Key]
    public int Id { get; set; }
    public string LivingStatus { get; set; }
    }
}