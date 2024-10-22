
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models.HomeProfile
{
    [Table("agegroup")]
    public class AgeGroupModel
    {
    [Key]
    public int Id { get; set; }
    public string Age { get; set; }
    }
}