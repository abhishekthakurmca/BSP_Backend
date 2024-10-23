using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Dto.Business;
public class BusinessUserDto
{
    //[Key]
    [Column("businessUserId")]
    public int BusinessUserId { get; set; }
    public int BusinessId { get; set; }
    [Column("business_name")]
    public string? BusinessName { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    [Column("first_name")]
    public string FirstName { get; set; }
    [Column("last_name")]
    public string? LastName { get; set; }
    public string? Postcode { get; set; }
}
