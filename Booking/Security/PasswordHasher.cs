using System.Security.Cryptography;

namespace Booking.Security
{
    public class PasswordHasher
    {
        public (string hashedPassword, string salt) HashPassword(string password)
        {
            byte[] saltBytes;
            new RNGCryptoServiceProvider().GetBytes(saltBytes = new byte[16]);
            string salt = Convert.ToBase64String(saltBytes);

            var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(saltBytes, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            string hashedPassword = Convert.ToBase64String(hashBytes);
            return (hashedPassword, salt);
        }


        public bool VerifyPassword(string enteredPassword, string savedPasswordHash, string savedSalt)
        {
            byte[] saltBytes = Convert.FromBase64String(savedSalt);

            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);

            var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, saltBytes, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }

    }
}
