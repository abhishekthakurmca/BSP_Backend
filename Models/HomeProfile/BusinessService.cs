using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models.HomeProfile;
[Table("business_service")]
public class BusinessService
{
    [Key]
    public long Id { get; set; }
    public long BusinessId { get; set; }
    public long ServiceId { get; set; }
}