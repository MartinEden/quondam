using System;
using System.Security.Cryptography;

namespace MartinEden.Quondam
{
    internal class PasswordRecord
    {
        private readonly string hashedPassword;
        private readonly string salt;
        private readonly int hashStrength;
        internal readonly DateTime Timestamp;

        // For a discussion of hashStrength, see PasswordRecordTests.HashingPasswordsCostsTheRightAmount
        internal PasswordRecord(string password, DateTime timestamp, int hashStrength)
        {
            this.hashStrength = hashStrength;
            salt = getSalt();
            hashedPassword = hashPassword(password, salt);
            Timestamp = timestamp;
        }

        private string getSalt()
        {
            byte[] salt = new byte[16]; // PBKDF2 standard recommends at least 8 bytes
            new RNGCryptoServiceProvider().GetBytes(salt);
            return Convert.ToBase64String(salt);
        }
        private string hashPassword(string password, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);
            var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, hashStrength);
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
