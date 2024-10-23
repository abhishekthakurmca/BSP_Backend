using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models.HomeProfile;


[Table("business_product")]
public class BusinessProducts
{
    [Key]
    public long Id { get; set; }
    public long BusinessId { get; set; }
    public long ProductId { get; set; }
}