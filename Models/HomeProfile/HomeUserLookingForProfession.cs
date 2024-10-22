
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models.HomeProfile
{
    [Table("homeuser_lookingfor_profession")]
    public class HomeUserLookingForProfession
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public int ProfessionId { get; set; }
    }

}