using System;
using System.Security.Cryptography;

namespace MartinEden.Quondam
{
    internal class PasswordRecord
    {
        private readonly string hashedPassword;
        private readonly string salt;
        internal readonly DateTime Timestamp;

        internal PasswordRecord(string password, DateTime timestamp)
        {
            salt = getSalt();
            hashedPassword = hashPassword(password, salt);
            Timestamp = timestamp;
        }

        private string getSalt()
        {
            byte[] salt = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(salt);
            return Convert.ToBase64String(salt);
        }
        private string hashPassword(string password, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);
            var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            return Convert.ToBase64String(hash);
        }

        internal bool ValidatePassword(string passwordToCheck)
        {
            return hashPassword(passwordToCheck, salt) == hashedPassword;
        }

        internal bool IsFresh(DateTime now)
        {
            return (now - Timestamp) <= PasswordManager.MaximumPasswordAge;
        }
    }
}
