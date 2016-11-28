using System;
using System.Collections.Generic;

namespace MartinEden.Quondam
{
    internal class UserStore
    {
        private Dictionary<string, PasswordRecord> records;

        internal UserStore()
        {
            records = new Dictionary<string, PasswordRecord>();
        }

        internal void StorePassword(string username, string password, DateTime timestamp)
        {
            records[username] = new PasswordRecord(password, timestamp);
        }

        internal PasswordRecord GetPassword(string username)
        {
            PasswordRecord record;
            if (records.TryGetValue(username, out record))
            {
                return record;
            }
            return null;
        }

        internal void ClearPassword(string username)
        {
            records.Remove(username);
        }
    }
}
