
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models.HomeProfile
{

[Table("homeuser_construction_interests")]
public class HomeUserConstructionInterest
{
    public long Id { get; set; }

    [Column("userid")]
    public long UserId { get; set; }

    [Column("cinterestid")]
    public int CInterestId { get; set; }
}

}