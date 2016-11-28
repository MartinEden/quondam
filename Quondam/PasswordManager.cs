using System;

namespace MartinEden.Quondam
{
    public class PasswordManager : IPasswordManager
    {
        private IClock clock;

        public PasswordManager()
            : this(new RealClock()) { }
        internal PasswordManager(IClock clock)
        {
            this.clock = clock;
        }

        public string GenerateOneTimePassword(string username)
        {
            throw new NotImplementedException();
        }

        public bool ValidatePassword(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}