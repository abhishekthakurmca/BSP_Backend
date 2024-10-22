
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models.HomeProfile
{

    public class BusinessProductRequest
    {
        public int BusinessId { get; set; }

        public List<int> ProductIds { get; set; } // The selected product IDs

    }

}