using System;
using System.Web.Security;

namespace MartinEden.Quondam
{
    public class PasswordManager : IPasswordManager
    {
        private IClock clock;
        private UserStore store;

        internal static readonly TimeSpan MaximumPasswordAge = TimeSpan.FromSeconds(30);

        public PasswordManager()
            : this(new RealClock()) { }
        internal PasswordManager(IClock clock)
        {
            this.clock = clock;
            store = new UserStore();
        }

        public string GenerateOneTimePassword(string username)
        {
            string password = Membership.GeneratePassword(16, 0);
            store.StorePassword(username, password, clock.Now);
            return password;
        }

        public bool ValidatePassword(string username, string password)
        {
            var record = store.GetPassword(username);
            if (record != null && record.IsFresh(clock.Now) && record.ValidatePassword(password))
            {
                store.ClearPassword(username);
                return true;
            }
            return false;
        }
    }
}