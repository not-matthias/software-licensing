using crypto;
using System;
using System.Security.Cryptography;

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
        public EncryptedPacket<CryptoData> Encrypt(RSAParameters key)
        {
            if (Data.GetType() == typeof(CryptoData))
            {
                throw new InvalidOperationException("Data is already encrypted");
            }

            var encryptedData = _cryptoManager.Encrypt(key, Data.ToByteArray());
            return new EncryptedPacket<CryptoData>(encryptedData);
        }

        /// <summary>
        /// Calculates the checksum of the data.
        /// </summary>
        /// <returns>The checksum as a string</returns>
        public string CalculateChecksum()
        {
            return _cryptoManager.GenerateHash(Data.ToByteArray());
        }

        /// <summary>
        /// Checks whether the packet is valid, by comparing the checksums.
        /// </summary>
        /// <returns>True if it's valid</returns>
        public bool IsValid()
        {
            return CalculateChecksum() == Checksum;
        }
    }
}
