using System.Text;

namespace Identity.Application.Common
{
    public static class EncryptionHelper
    {
        public static (string Hash, string Salt) HashPassword(string password)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA256();
            var salt = Convert.ToBase64String(hmac.Key);
            var hash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));

            return (hash, salt);
        }

        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA256(Convert.FromBase64String(storedSalt));
            var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));

            return storedHash == computedHash;
        }
    }
}
