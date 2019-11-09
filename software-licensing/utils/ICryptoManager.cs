using System.Security.Cryptography;

namespace crypto
{
    public interface ICryptoManager
    {
        public (RSAParameters publicKey, RSAParameters privateKey) GenerateKeyPair();

        public byte[] Encrypt(RSAParameters key, byte[] data);

        public byte[] Decrypt(RSAParameters key, byte[] encryptedData);

        public string GenerateHash(string data);
    }
}