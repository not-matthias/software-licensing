using System.Security.Cryptography;
using System.Text;

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

        public byte[] Encrypt(RSAParameters key, byte[] data)
        {
            using (var rsa = new RSACng())
            {
                // Import given public key to RSA
                rsa.ImportParameters(key);

                // Return encrypted data
                return rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA512);
            }
        }

        public byte[] Decrypt(RSAParameters key, byte[] encryptedData)
        {
            using (var rsa = new RSACng())
            {
                // Import given public key to RSA
                rsa.ImportParameters(key);

                // Return decrypted data
                return rsa.Decrypt(encryptedData, RSAEncryptionPadding.OaepSHA512);
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
