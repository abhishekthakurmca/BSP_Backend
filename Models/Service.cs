using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models;

public class Service
{
    [Key]
    [Column("service_id")]
    public int ServiceId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}

