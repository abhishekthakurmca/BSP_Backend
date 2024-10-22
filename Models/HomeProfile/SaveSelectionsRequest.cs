
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models.HomeProfile
{
    public class SaveSelectionsRequest
    {
        public long UserId { get; set; }
        public List<int> ProfessionIds { get; set; }
        public List<int> ServiceIds { get; set; }
        public List<int> SupplierTypeIds { get; set; }
        public List<int> AncillaryIds { get; set; }
    }
}