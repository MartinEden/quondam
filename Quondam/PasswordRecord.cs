using System;

namespace MartinEden.Quondam
{
    internal class PasswordRecord
    {
        internal readonly string Password;
        internal readonly DateTime Timestamp;

        internal PasswordRecord(string password, DateTime timestamp)
        {
            Password = password;
            Timestamp = timestamp;
        }

        internal bool IsFresh(DateTime now)
        {
            return (now - Timestamp) <= PasswordManager.MaximumPasswordAge;
        }
    }
}
