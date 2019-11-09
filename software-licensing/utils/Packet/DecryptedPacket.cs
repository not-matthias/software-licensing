using crypto;
using System.Security.Cryptography;

namespace utils
{
    public class DecryptedPacket<T> : Packet
    {
        private ICryptoManager _cryptoManager;

        public T Data { get; set; }

        public DecryptedPacket(ICryptoManager cryptoManager, T data)
        {
            _cryptoManager = cryptoManager;

            Data = data;
            Checksum = cryptoManager.GenerateHash(ToByteArray(Data));
        }

        public EncryptedPacket<T> Encrypt(RSAParameters key)
        {
            var encryptedData = _cryptoManager.Decrypt(key, ToByteArray(Data));

            EncryptedPacket<T> packet = new EncryptedPacket<T>(_cryptoManager)
            {
                Data = encryptedData,
                Checksum = _cryptoManager.GenerateHash(encryptedData)
            };

            return packet;
        }
    }
}
