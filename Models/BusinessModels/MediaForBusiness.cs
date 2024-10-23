using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models.BusinessNameSpace;

public class MediaForBusiness
{
    [Key]
        [Column("mediaid")]
    public long MediaId { get; set; }
    public long BusinessId { get; set; }
    public DateTime DateCreated { get; set; }
    public string? MediaType { get; set; }
    public string? MediaUrl { get; set; }
}
