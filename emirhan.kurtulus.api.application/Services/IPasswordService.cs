namespace emirhan.kurtulus.api.application.Services;

public interface IPasswordService
{
    string HashPassword(string value);

    bool VerifyPassword(string password, string hashedPassword);
}