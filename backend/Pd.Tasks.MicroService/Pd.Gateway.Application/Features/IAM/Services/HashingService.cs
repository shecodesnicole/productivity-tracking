using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Pd.Tasks.Application.Features.IAM.Services
{
    internal static class HashingService
    {
        public static string Hash(string password)
        {
            // STEP 1 Create the salt value with a cryptographic PRNG
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            // STEP 2 Create the Rfc2898DeriveBytes and get the hash value
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            // STEP 3 Combine the salt and password bytes for later use
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            // STEP 4 Turn the combined salt+hash into a string for storage
            return Convert.ToBase64String(hashBytes);
        }

        public static bool IsHashOf(string hashedPassord, string password)
        {
            /* Extract the bytes */
            byte[] hashBytes = Convert.FromBase64String(hashedPassord);

            /* Get the salt */
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            /* Compute the hash on the password the user entered */
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            /* Compare the results */
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;

            return true;
        }
    }
}