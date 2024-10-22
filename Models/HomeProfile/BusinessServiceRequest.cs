
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models.HomeProfile
{

    public class BusinessServiceRequest
    {
        public int BusinessId { get; set; }

        public List<int> ServiceIds { get; set; } // The selected profession IDs

    }

}