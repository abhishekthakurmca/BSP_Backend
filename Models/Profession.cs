using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models;

public class Profession
{
    [Key]
    [Column("profession_id")]
    public int ProfessionId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}