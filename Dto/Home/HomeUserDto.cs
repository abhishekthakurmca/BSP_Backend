using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyBackendApp.Dto.Home;

public class HomeUserDto
{
    public int UserId { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Pwd { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Postcode { get; set; }
    public string? Suburb { get; set; }
    public Guid? ActivationToken { get; set; }
}
