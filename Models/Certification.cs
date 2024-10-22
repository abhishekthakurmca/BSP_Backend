using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models
{
    [Table("certification")]
    public class Certification
    {
        [Key] // Marks this as the primary key
        public int CertificationId { get; set; }
        public int ProfessionId { get; set; } // Upd your table has an Id column
        public string Name { get; set; }
        // Add other columns from your table as needed
    }
}