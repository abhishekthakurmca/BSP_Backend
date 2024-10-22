using System.Net.Mail;

public interface IEmailService
{
    Task SendActivationEmailAsync(string email, string activationLink);
}

public class DummyEmailService : IEmailService
{
    public Task SendActivationEmailAsync(string email, string activationLink)
    {
        // Log the email sending attempt or do nothing
        Console.WriteLine($"Pretending to send an email to {email} with activation link {activationLink}");
        return Task.CompletedTask;
    }
}
/*
public interface IEmailService
{
    Task SendActivationEmailAsync(string email, string activationLink);
}

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendActivationEmailAsync(string email, string activationLink)
    {
        var fromAddress = _configuration["EmailSettings:From"];
        var smtpClient = new System.Net.Mail.SmtpClient(_configuration["EmailSettings:SmtpHost"])
        {
            Port = int.Parse(_configuration["EmailSettings:SmtpPort"]),
            Credentials = new System.Net.NetworkCredential(_configuration["EmailSettings:SmtpUser"], _configuration["EmailSettings:SmtpPass"]),
            EnableSsl = true,
        };

        var mailMessage = new MailMessage
        {
            From = new System.Net.Mail.MailAddress(fromAddress),
            Subject = "Activate your account",
            Body = $"Please activate your account by clicking <a href=\"{activationLink}\">here</a>.",
            IsBodyHtml = true,
        };

        mailMessage.To.Add(email);

        await smtpClient.SendMailAsync(mailMessage);
    }
}

*/