namespace Checkout.com.Common.Encryption
{
    public interface IEncryptionService
    {
        string Encrypt(string value, string key);

        string Decrypt(string value, string key);
    }
}
