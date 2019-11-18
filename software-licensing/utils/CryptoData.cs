using System;

namespace utils
{
    [Serializable]
    public class CryptoData
    {
        public byte[] EncryptedKey { get; set; }
        public byte[] EncryptedIV { get; set; }
        public byte[] EncryptedData { get; set; }

        public CryptoData()
        {
        }
    }
}
