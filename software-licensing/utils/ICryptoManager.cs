using System.Security.Cryptography;
using utils;

namespace crypto
{
    public interface ICryptoManager
    {
        public (RSAParameters publicKey, RSAParameters privateKey) GenerateKeyPair();

        public CryptoData Encrypt(RSAParameters rsaKey, byte[] data);

        public byte[] Decrypt(RSAParameters rsaKey, CryptoData cryptoData);

        public string GenerateHash(string data);

        public string GenerateHash(byte[] data);
    }
}