namespace MyBackendApp.IServices;

public interface IEmailService
{
    Task SendActivationEmailAsync(string Email, Guid? ActivationCode);
}
