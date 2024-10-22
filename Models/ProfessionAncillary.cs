using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models
{
    [Table("profession_ancillary")]
    public class ProfessionAncillary
    {
        [Key] // Marks this as the primary key
        [Column("ancillary_id")] // Maps the property to the correct column in the database
        public int AncillaryId { get; set; } // Upd your table has an Id column
        public string Name { get; set; }
        // Add other columns from your table as needed
    }
}