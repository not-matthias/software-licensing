﻿using System;
using System.Security.Cryptography;

namespace utils
{
    [Serializable]
    public class EncryptedPacket<T> : Packet<T> where T : CryptoData
    {
        /// <summary>
        /// Parameterless constructor for the serialization.
        /// </summary>
        public EncryptedPacket()
        {
        }

        /// <summary>
        /// Creates a new packet with the data and calculates the checksum.
        /// </summary>
        /// <param name="data">The encrypted data of the packet</param>
        public EncryptedPacket(T data)
        {
            Data = data;
            Checksum = CalculateChecksum();
        }

        /// <summary>
        /// Decrypts the data of the packet.
        /// </summary>
        /// <typeparam name="V">The type of the decrypted data</typeparam>
        /// <param name="key">The asymmetric key</param>
        /// <returns>The decrypted data of the packet</returns>
        public V Decrypt<V>(RSAParameters key)
        {
            return _cryptoManager.Decrypt(key, Data).FromByteArray<V>();
        }
    }
}
