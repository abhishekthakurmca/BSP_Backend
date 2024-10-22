
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models.HomeProfile
{

    public class BusinessOnboardingRequest
    {
        public int BusinessId { get; set; }
        public string BusinessType { get; set; }
        public int Residential { get; set; }
        public int Commercial { get; set; }
        public int Government { get; set; }

        
        public List<int> ProfessionIds { get; set; } // The selected profession IDs
        public List<int> AncillaryIds { get; set; }  // The selected ancillary IDs
    }

}