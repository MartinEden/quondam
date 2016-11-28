namespace MartinEden.Quondam
{
    public interface IPasswordManager
    {
        /// <summary>
        /// Generate a 16 character password for any given username. This password
        /// is guaranteed to be distinct from any other user's password.
        /// </summary>
        string GenerateOneTimePassword(string username);
        /// <summary>
        /// Returns true if the provided password was the one generated for the user
        /// the last time GenerateOneTimePassword was called for that user. Only
        /// returns true if the password was generated within the last 30 seconds.
        /// </summary>
        bool ValidatePassword(string username, string password);        
    }
}