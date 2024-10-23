namespace MyBackendApp.Dto.PersonalInterest;

public class PersonalInterestRequestDto
{
    public long UserId { get; set; }
    public List<int> PInterestIds { get; set; }
}
