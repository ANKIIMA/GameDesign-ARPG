namespace SharpUI.Source.Common.Util.Encryption
{
    public interface IPasswordUtil
    {
        string PasswordHash(string password);
        
        bool IsPasswordValid(string password, string hashedPassword);
    }
}