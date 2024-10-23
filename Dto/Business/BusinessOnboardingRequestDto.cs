namespace MyBackendApp.Dto.Business;

public class BusinessOnboardingRequestDto
{    public int BusinessId { get; set; }
    public string BusinessType { get; set; }
    public int Residential { get; set; }
    public int Commercial { get; set; }
    public int Government { get; set; }
    public List<int> ProfessionIds { get; set; }
    public List<int> AncillaryIds { get; set; }
}
