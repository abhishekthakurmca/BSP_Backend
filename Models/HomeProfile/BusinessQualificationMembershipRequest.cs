
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models.HomeProfile
{

    public class BusinessQualificationMembershipRequest
    {
        public int BusinessId { get; set; }
        public string? RegistrationNumber { get; set; }
        public int? YearsExperience { get; set; }
        public List<int>? QualificationIds { get; set; }
        public List<int>? MembershipIds { get; set; }

    }

}