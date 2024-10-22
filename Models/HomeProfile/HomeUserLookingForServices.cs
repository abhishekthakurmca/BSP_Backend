
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models.HomeProfile
{
    [Table("homeuser_lookingfor_service")]

    public class HomeUserLookingForServices
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        
        public int ServiceId { get; set; }
    }

}