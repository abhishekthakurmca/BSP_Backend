using System.ComponentModel.DataAnnotations;

namespace MyBackendApp.Models;

public class HomeUser
{
    [Key]
    //[Column("user_id")]
    public int user_id { get; set; }
    public string Email { get; set; }
    public string Pwd { get; set; }
    public string? Phone { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Postcode { get; set; }
    public string? Suburb { get; set; }
    public Guid? ActivationToken { get; set; }
}