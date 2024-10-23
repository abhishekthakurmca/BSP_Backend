using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models
{
    [Table("qualification")]
    public class Qualification
    {
        [Key]
        public int QualificationId { get; set; }
        public int ProfessionId { get; set; }
        public string Name { get; set; }
    }
}