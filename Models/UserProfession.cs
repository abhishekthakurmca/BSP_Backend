using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models;

public class UserProfession
{
    [Key]
    public int Id { get; set; }
    [Column("user_id")]
    public int UserId { get; set; }
    [Column("profession_id")]
    public int ProfessionId { get; set; }
}