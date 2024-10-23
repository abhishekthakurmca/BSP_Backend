using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models;

[Table("profession_ancillary")]
public class ProfessionAncillary
{
    [Key] 
    [Column("ancillary_id")]
    public int AncillaryId { get; set; } 
    public string Name { get; set; }
}