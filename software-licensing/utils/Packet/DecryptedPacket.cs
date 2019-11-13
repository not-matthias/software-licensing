using crypto;
using System.Security.Cryptography;

namespace utils
{
    public class DecryptedPacket<T> : Packet<T>
    {
        private readonly ICryptoManager _cryptoManager;

        public DecryptedPacket()
        {
        }

        public DecryptedPacket(ICryptoManager cryptoManager, T data)
        {
            _cryptoManager = cryptoManager;

            Data = data;
            Checksum = GenerateChecksum();
        }

        public string GenerateChecksum()
        {
            if (_cryptoManager == null) return null;

            return _cryptoManager.GenerateHash(ToByteArray(Data));
        }

        public EncryptedPacket<T> Encrypt(ICryptoManager cryptoManager, RSAParameters key)
        {
            if (_cryptoManager == null) return null;

            var encryptedData = cryptoManager.Encrypt(key, ToByteArray(Data));
            return new EncryptedPacket<T>(cryptoManager, encryptedData);
        }
    }
}
