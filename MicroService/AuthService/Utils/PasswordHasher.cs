using System.Security.Cryptography;
using System.Text;

namespace AuthService.Utils
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                // Parolayı byte array'e dönüştürme
                var byteArray = Encoding.UTF8.GetBytes(password);
                // SHA256 ile hash oluşturma
                var hash = sha256.ComputeHash(byteArray);

                // Hash'i Base64 string'e dönüştürme ve geri döndürme
                return Convert.ToBase64String(hash);
            }
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // Parolayı yeniden hash'leyip verilen hash ile karşılaştırma
            var passwordHash = HashPassword(password);
            return passwordHash == hashedPassword;
        }
    }
}
