
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models
{
public class Service
{
    [Key] // Marks this as the primary key
    [Column("service_id")] // Map
    public int ServiceId { get; set; } // Assuming your table has an Id column
    public string? Name { get; set; }
    
    public string? Description { get; set; }
}
}

