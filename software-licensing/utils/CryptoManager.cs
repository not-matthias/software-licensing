using System.Security.Cryptography;
using System.Text;

namespace crypto
{
    public class CryptoManager : ICryptoManager
    {
        //public (RSAParameters publicKey, RSAParameters publicPrivateKey) GenerateKeyPair()
        //{
        //    // Create object implementing RSA. Note that this version of the
        //    // constructor generates a random 2048-bit key pair.
        //    using (var rsa = new RSACng())
        //    {
        //        return (publicKey: rsa.ExportParameters(false),
        //            publicPrivateKey: rsa.ExportParameters(true));
        //    }
        //}
        //https://github.com/rstropek/SecureCodingDotNet/blob/master/02-Asymmetric-Encryption/Program.cs

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

        // Generate public and private key
        // Encrypt data (asymmetric)
        // Decrypt data (asymmetric)
    }
}
