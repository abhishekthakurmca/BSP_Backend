namespace MyBackendApp.Dto.Business;

public class BusinessCertificationRequestDto
{
    public int BusinessId { get; set; }
    public string? LicenseNumber { get; set; }
    public DateTime? LicenseExpiry { get; set; }
    public bool? FullyInsured { get; set; }
    public List<int>? CertificationIds { get; set; }
}
