
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models.HomeProfile
{

    public class BusinessCertificationRequest
    {
    public int BusinessId { get; set; }
    public string? LicenseNumber { get; set; }
    public DateTime? LicenseExpiry { get; set; }
    public bool? FullyInsured { get; set; }
    public List<int>? CertificationIds { get; set; }

    }

}