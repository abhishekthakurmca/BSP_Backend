using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models;

[Table("industrymembership")]
public class IndustryMembership
{
    [Key]
    public int MembershipId { get; set; }
    public int ProfessionId { get; set; }
    public string Name { get; set; }
}