using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyBackendApp.Dto.Home;

public class HomeUserDto
{
    [Key]
    [Column("user_id")]
    public int UserId { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}
