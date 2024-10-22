
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models.HomeProfile
{
    [Table("homeuser_personal_interests")]
    public class HomeUserPersonalInterest
    {
        public long Id { get; set; }

        [Column("userid")]
        public long UserId { get; set; }

        [Column("pinterestid")]
        public int PInterestId { get; set; }
    }

}