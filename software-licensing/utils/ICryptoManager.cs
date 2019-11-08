using System.Security.Cryptography;

namespace crypto
{
    public interface ICryptoManager
    {
        public (RSAParameters publicKey, RSAParameters publicPrivateKey) GenerateKeyPair();

        public byte[] Encrypt(RSAParameters publicKey, byte[] data);

        public byte[] Decrypt(RSAParameters publicKey, byte[] encryptedData);

        public string GenerateHash(string data);
    }
}