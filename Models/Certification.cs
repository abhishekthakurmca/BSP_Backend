using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models;

[Table("certification")]
public class Certification
{
    [Key]
    public int CertificationId { get; set; }
    public int ProfessionId { get; set; }
    public string Name { get; set; }
}