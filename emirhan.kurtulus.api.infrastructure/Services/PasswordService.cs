using emirhan.kurtulus.api.application.Services;

namespace emirhan.kurtulus.api.infrastructure.Services;

public class PasswordService : IPasswordService
{
    public string HashPassword(string value)
    {
        return BCrypt.Net.BCrypt.HashPassword(value, BCrypt.Net.BCrypt.GenerateSalt());
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}