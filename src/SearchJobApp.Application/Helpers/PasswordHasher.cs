namespace SearchJobApp.Application.Helpers;

public static class PasswordHasher
{
    public static string HashPassword(string pasword) => BCrypt.Net.BCrypt.HashPassword(pasword);

    public static bool VerifyPassword(string password, string passwordHash) =>
        BCrypt.Net.BCrypt.Verify(password, passwordHash);
}