namespace MyBackendApp.Dto.Construction;

public class ConstructionInterestRequestDto
{
    public long UserId { get; set; }
    public List<int> CInterestIds { get; set; }
}
