
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models.HomeProfile
{
    [Table("interests_construction")]
    public class ConstructionInterest
    {
        [Key]
        [Column("cinterestid")]
        public int CInterestId { get; set; }

        [Column("interestname")]
        public string InterestName { get; set; }
        }

}