
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models.HomeProfile
{
    [Table("interests_personal")]
    public class PersonalInterest
    {
        [Key]
        [Column("pinterestid")]
        public int PInterestId { get; set; }

        [Column("interestname")]
        public string InterestName { get; set; }
        }

}