namespace MyBackendApp.Dto.Profession;

public class UserProfessionsDto
{
    public int UserId { get; set; }
    public List<int> ProfessionIds { get; set; }
}
