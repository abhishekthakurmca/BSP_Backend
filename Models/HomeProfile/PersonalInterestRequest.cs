
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models.HomeProfile
{
public class PersonalInterestRequest
{
    public long UserId { get; set; }
    public List<int> PInterestIds { get; set; } // List of selected personal interests
}
}