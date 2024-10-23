using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyBackendApp.Models;

public class BusinessUserModel
{
    [Key]
    public int businessUserId { get; set; }
    public int businessId { get; set; }
    [Column("business_name")]
    public string? BusinessName { get; set; }
    public string Email { get; set; }
    public string Pwd { get; set; }
    public string? Phone { get; set; }
    [Column("first_name")]
    public string? FirstName { get; set; }
    [Column("last_name")]
    public string? LastName { get; set; }
    public string? Postcode { get; set; }
}
