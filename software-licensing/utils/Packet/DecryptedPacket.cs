using crypto;
using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace utils
{
    public class DecryptedPacket<T> : Packet<T>
    {
        private readonly ICryptoManager _cryptoManager;

        public DecryptedPacket(SerializationInfo information, StreamingContext context)
        {
            Data = (T)information.GetValue("Data", typeof(T));
            Checksum = information.GetString("Checksum");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Data", Data);
            info.AddValue("Checksum", Checksum);
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
