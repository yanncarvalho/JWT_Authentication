namespace JwtAuthentication.Auth
{
    using BCrypt.Net;

    public class BCryptPassword
    {
        public static string HashGeneration(string password)
        {
            return BCrypt.HashPassword(password); 
        }

        public static bool PasswordCompare(string hash, string password)
        {
            return BCrypt.Verify(hash: hash, text: password);
        }
    }
}
