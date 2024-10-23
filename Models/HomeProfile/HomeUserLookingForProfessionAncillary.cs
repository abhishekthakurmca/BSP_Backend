using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models.HomeProfile;

[Table("homeUser_lookingFor_professionAncillary")]
public class HomeUserLookingForProfessionAncillary
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public int AncillaryId { get; set; }
}