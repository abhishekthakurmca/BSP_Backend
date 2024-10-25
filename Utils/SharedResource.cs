using Org.BouncyCastle.Asn1.Mozilla;
using System.Security.Cryptography;
using System.Text;

namespace MyBackendApp.Utils;

public static class SharedResource
{
    public static string UserAlreadyExist = "Email Address already registered !!";
    public static string UserRegisteredCheckEmail = "User registered successfully. Please check your email to activate your account.";

    public static bool VerifyPassword(string password, string storedHash)
    {
        string hashedPassword = HashPassword(password);
        return hashedPassword == storedHash;
    }

    public static string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
    public static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
