using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models
{
    [Table("supplier_type")]
    public class SupplierType
    {
        [Key] // Marks this as the primary key
        [Column("supplier_type_id")] // Maps the property to the correct column in the database
        public int SupplierTypeId { get; set; } // Upd your table has an Id column
        public string Name { get; set; }
        // Add other columns from your table as needed
    }
}