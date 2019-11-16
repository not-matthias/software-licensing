using crypto;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace utils
{
    [Serializable]
    public class Packet<T>
    {
        [NonSerialized]
        protected readonly CryptoManager _cryptoManager = new CryptoManager();

        public T Data { get; set; }
        public string Checksum { get; set; }

        /// <summary>
        /// Parameterless constructor for the serialization.
        /// </summary>
        public Packet()
        {
        }

        /// <summary>
        /// Creates a new packet with the data and calculates the checksum.
        /// </summary>
        /// <param name="data">The data of the packet</param>
        public Packet(T data)
        {
            Data = data;
            Checksum = CalculateChecksum();
        }

        /// <summary>
        /// Encrypts the data of the packet.
        /// </summary>
        /// <param name="key">The asymmetric key</param>
        /// <returns>The encrypted packet</returns>
        public async Task<EncryptedPacket<CryptoData>> EncryptAsync(RSAParameters key)
        {
            if (Data.GetType() == typeof(CryptoData))
            {
                throw new InvalidOperationException("Data is already encrypted");
            }

            var encryptedData = _cryptoManager.Encrypt(key, ToByteArray(Data));
            return new EncryptedPacket<CryptoData>(encryptedData);
        }

        /// <summary>
        /// Calculates the checksum of the data.
        /// </summary>
        /// <returns>The checksum as a string</returns>
        public string CalculateChecksum()
        {
            return _cryptoManager.GenerateHash(ToByteArray(Data));
        }

        /// <summary>
        /// Checks whether the packet is valid, by comparing the checksums.
        /// </summary>
        /// <returns>True if it's valid</returns>
        public bool IsValid()
        {
            return CalculateChecksum() == Checksum;
        }

        public static byte[] ToByteArray<V>(V obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            using MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }

        public static V FromByteArray<V>(byte[] data)
        {
            if (data == null)
                return default;

            BinaryFormatter bf = new BinaryFormatter();
            using MemoryStream ms = new MemoryStream(data);
            object obj = bf.Deserialize(ms);
            return (V)obj;
        }
    }
}
