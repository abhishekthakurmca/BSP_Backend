using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models.HomeProfile;


[Table("business_membership")]
public class BusinessMembership
{
    [Key]
    public int Id { get; set; }
    public int BusinessId { get; set; }
    public int MembershipId { get; set; }
}