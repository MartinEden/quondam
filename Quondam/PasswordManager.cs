using System;

namespace MartinEden.Quondam
{
    public class PasswordManager : IPasswordManager
    {
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