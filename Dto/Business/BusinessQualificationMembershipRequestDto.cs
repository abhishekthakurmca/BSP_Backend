namespace MyBackendApp.Dto.Business;

public class BusinessQualificationMembershipRequestDto
{
    public int BusinessId { get; set; }
    public string? RegistrationNumber { get; set; }
    public int? YearsExperience { get; set; }
    public List<int>? QualificationIds { get; set; }
    public List<int>? MembershipIds { get; set; }
}
