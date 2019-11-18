using System.IO;
using System.Security.Cryptography;
using System.Text;
using utils;

namespace crypto
{
    public class CryptoManager : ICryptoManager
    {
        public (RSAParameters publicKey, RSAParameters privateKey) GenerateKeyPair()
        {
            // Create object implementing RSA. Note that this version of the
            // constructor generates a random 2048-bit key pair.
            using (var rsa = new RSACng())
            {
                return (
                        publicKey: rsa.ExportParameters(false),
                        privateKey: rsa.ExportParameters(true)
                    );
            }
        }

        public CryptoData Encrypt(RSAParameters rsaKey, byte[] data)
        {
            byte[] encryptedData;
            byte[] key;
            byte[] iv;

            //
            // Use AES to encrypt the data
            //
            using (var aes = new AesCng())
            {
                key = aes.Key;
                iv = aes.IV;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    encryptedData = PerformCryptography(data, encryptor);
                }
            }

            //
            // Use RSA to encrypt AES key/IV
            //
            using (var rsa = new RSACng())
            {
                // Import given public key to RSA
                rsa.ImportParameters(rsaKey);

                return new CryptoData
                {
                    EncryptedKey = rsa.Encrypt(key, RSAEncryptionPadding.OaepSHA512),
                    EncryptedIV = rsa.Encrypt(iv, RSAEncryptionPadding.OaepSHA512),
                    EncryptedData = encryptedData
                };
            }
        }

        public byte[] Decrypt(RSAParameters rsaKey, CryptoData cryptoData)
        {
            using (var rsa = new RSACng())
            {
                // Import key pair
                rsa.ImportParameters(rsaKey);

                // Decrypt key/IV using asymmetric RSA algorithm
                var key = rsa.Decrypt(cryptoData.EncryptedKey, RSAEncryptionPadding.OaepSHA512);
                var iv = rsa.Decrypt(cryptoData.EncryptedIV, RSAEncryptionPadding.OaepSHA512);

                // Use symmetric AES algorithm to decrypt secret message
                using (var aes = new AesCng())
                {
                    aes.Key = key;
                    aes.IV = iv;

                    using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    {
                        return PerformCryptography(cryptoData.EncryptedData, decryptor);
                    }
                }
            }
        }

        private byte[] PerformCryptography(byte[] data, ICryptoTransform cryptoTransform)
        {
            using (var ms = new MemoryStream())
            using (var cryptoStream = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write))
            {
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();

                return ms.ToArray();
            }
        }

        public string GenerateHash(string data)
        {
            using (var sha512 = new SHA512Managed())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                byte[] hash = sha512.ComputeHash(bytes);
                return GetStringFromHash(hash);
            }
        }

        public string GenerateHash(byte[] data)
        {
            if (data is null)
            {
                return default;
            }

            using (var sha512 = new SHA512Managed())
            {
                byte[] hash = sha512.ComputeHash(data);
                return GetStringFromHash(hash);
            }
        }

        private string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }

    }
}
