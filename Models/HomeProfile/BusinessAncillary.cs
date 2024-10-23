using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models.HomeProfile;


[Table("business_ancillary")]
public class BusinessAncillary
{
    [Key]
    public long Id { get; set; }

    public long BusinessId { get; set; }

    public long AncillaryId { get; set; }

    //public Business Business { get; set; }
    //public Ancillary Ancillary { get; set; }
}