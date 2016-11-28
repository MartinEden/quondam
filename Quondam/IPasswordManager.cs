namespace MartinEden.Quondam
{
    public interface IPasswordManager
    {
        string GenerateOneTimePassword(string username);
        bool ValidatePassword(string username, string password);        
    }
}