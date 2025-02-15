using System.Security.Cryptography;

namespace FinanceApp.Helpers;

public class PasswordHasher
{
    public static string HashPassword(string password)
    {
        // Salt: A random value to add additional security
        using (var rng = new RNGCryptoServiceProvider())
        {
            byte[] salt = new byte[16]; // 128-bit salt
            rng.GetBytes(salt);

            // PBKDF2 with SHA256
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000)) // 10,000 iterations
            {
                byte[] hash = pbkdf2.GetBytes(20); // 20-byte hash
                byte[] hashBytes = new byte[36]; // Salt + hash

                // Combine salt and hash
                Array.Copy(salt, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 20);

                // Return the result as a base64 string
                return Convert.ToBase64String(hashBytes);
            }
        }
    }

    public static bool VerifyPassword(string enteredPassword, string storedHash)
    {
        byte[] hashBytes = Convert.FromBase64String(storedHash);

        // Extract salt from stored hash
        byte[] salt = new byte[16];
        Array.Copy(hashBytes, 0, salt, 0, 16);

        // Hash the entered password with the same salt
        using (var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, 10000))
        {
            byte[] hash = pbkdf2.GetBytes(20);

            // Compare the hashes
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i]) return false;
            }

            return true;
        }
    }
}