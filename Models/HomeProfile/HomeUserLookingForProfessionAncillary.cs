
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models.HomeProfile
{
    [Table("homeuser_lookingfor_professionancillary")]
    public class HomeUserLookingForProfessionAncillary
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public int AncillaryId { get; set; }
    }
}