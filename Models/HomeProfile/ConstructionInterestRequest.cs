
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models.HomeProfile
{
    public class ConstructionInterestRequest
    {
        public long UserId { get; set; }
        public List<int> CInterestIds { get; set; } // List of selected construction interests
    }
}