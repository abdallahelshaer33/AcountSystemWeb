

using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;

namespace StoreManagementSystem.Dtos
{
    public class PasswordHash
    {
        public static string Hash(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var Bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(Bytes);
                return Convert.ToBase64String(hash);
            }

        }

        public static bool verify(string password, string storedHash)
        {
            var hashofInput = Hash(password);
            return hashofInput == storedHash;
        }
    }

}