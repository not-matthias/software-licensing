using crypto;
using System.Security.Cryptography;
using System.Text.Json;

namespace utils
{
    public class EncryptedPacket<T> : Packet
    {
        private ICryptoManager _cryptoManager;

        public byte[] Data { get; set; }

        public EncryptedPacket(ICryptoManager cryptoManager)
        {
            _cryptoManager = cryptoManager;
        }

        public T Decrypt(RSAParameters key)
        {
            return FromByteArray<T>(_cryptoManager.Decrypt(key, Data));
        }

        public bool IsValid()
        {
            return JsonSerializer.Serialize(Data) == Checksum;
        }
    }
}
