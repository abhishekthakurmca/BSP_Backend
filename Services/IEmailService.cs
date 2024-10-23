namespace MyBackendApp.Services;

public interface IEmailService
{
    Task SendActivationEmailAsync(string email, string activationLink);
}
