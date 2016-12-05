using NLog;
using System;
using System.Web.Security;

namespace MartinEden.Quondam
{
    public class PasswordManager : IPasswordManager
    {
        internal static readonly TimeSpan MaximumPasswordAge = TimeSpan.FromSeconds(30);
        internal const int DefaultHashStrength = 100000;
        private static Logger logger = LogManager.GetLogger("PasswordAudit");

        private IClock clock;
        private UserStore store;

        public PasswordManager()
            : this(new RealClock(), DefaultHashStrength) { }
        internal PasswordManager(IClock clock, int hashStrength)
        {
            this.clock = clock;
            store = new UserStore(hashStrength);
        }

        public string GenerateOneTimePassword(string username)
        {
            string password = Membership.GeneratePassword(16, 0);
            store.StorePassword(username, password, clock.Now);
            logger.Info("Generated password for user '{0}'", username);
            return password;
        }

        public bool ValidatePassword(string username, string password)
        {
            var record = store.GetPassword(username);
            if (record != null && record.IsFresh(clock.Now) && record.ValidatePassword(password))
            {
                store.ClearPassword(username);
                logger.Info("Successful login for user '{0}'", username);
                return true;
            }
            else
            {
                logger.Warn("Failed login attempt for user '{0}' using bad password '{1}'", username, password);
                return false;
            }
        }
    }
}