namespace Tar.Cryptography
{
    public interface IHashing
    {
        void GetHashAndSalt(byte[] data, out byte[] hash, out byte[] salt);

        void GetHashAndSaltString(string data, out string hash, out string salt);

        bool VerifyHash(byte[] data, byte[] hash, byte[] salt);

        bool VerifyHashString(string data, string hash, string salt);

        void Encrypt(byte[] data, out byte[] encrypted, out byte[] saltKey);
        void EncryptString(string data, out string encrypted, out string saltKey);

        void Decrypt(byte[] encrypted, byte[] saltKey, out byte[] data);

        void DecryptString(string encrypted, string saltKey, out string data);

        bool VerifyEncryption(byte[] data, byte[] saltKey, byte[] encrypted);

        bool VerifyEncryptionString(string data, string saltKey, string encrypted);

        void Release();

        string Encrypt(string data, string saltKey);
    }
}