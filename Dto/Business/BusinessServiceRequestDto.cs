namespace MyBackendApp.Dto.Business;

public class BusinessServiceRequestDto
{
    public int BusinessId { get; set; }
    public List<int> ServiceIds { get; set; }
}
