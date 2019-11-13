using crypto;
using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace utils
{
    public class EncryptedPacket<T> : Packet<byte[]>
    {
        private ICryptoManager _cryptoManager;

        public EncryptedPacket(SerializationInfo information, StreamingContext context)
        {
            Data = (byte[])information.GetValue("Data", typeof(byte[]));
            Checksum = information.GetString("Checksum");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Data", Data);
            info.AddValue("Checksum", Checksum);
        }

        public EncryptedPacket(ICryptoManager cryptoManager, byte[] data)
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

        public T Decrypt(RSAParameters key)
        {
            return FromByteArray<T>(_cryptoManager.Decrypt(key, Data));
        }
    }
}
