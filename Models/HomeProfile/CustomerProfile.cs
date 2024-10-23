using MyBackendApp.Models.BusinessModels;
using MyBackendApp.Models.Jobs;

namespace MyBackendApp.Models.HomeProfile
{
    public class CustomerProfileModel
    {
        public HomeUser User { get; set; }
        public List<JobModel> Jobs { get; set; }
        public List<ConstructionInterest>? ConstructionInterests { get; set; }
        public List<ReviewOfBusiness> BusinessReviews { get; set; }
    }
}