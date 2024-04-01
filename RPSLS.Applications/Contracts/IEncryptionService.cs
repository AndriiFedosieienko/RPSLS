namespace RPSLS.Applications.Contracts
{
    public interface IEncryptionService
    {
        Task<string> Encrypt(string clearText);

        Task<string> Decrypt(string encrypted);
    }
}
