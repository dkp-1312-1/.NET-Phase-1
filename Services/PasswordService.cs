using BCrypt.Net;
public class PasswordService
{
    private const int WorkFactor = 12; 
    public string HashPassword(string plainPassword)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(plainPassword, WorkFactor);
    }

    // 2. Verify the password during login
    public bool VerifyPassword(string plainPassword, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(plainPassword, hashedPassword);
    }
}