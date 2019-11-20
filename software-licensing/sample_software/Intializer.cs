using crypto;
using System;
using utils;

namespace sample_software
{
    public class Intializer
    {
        public static CryptoManager cryptoManager = new CryptoManager();

        public static void Init()
        {
            Console.WriteLine("Enter your data to encrypt:");
            var userInput = Console.ReadLine();

            var byteData = userInput.ToByteArray();

            var (publicKey, privateKey) = cryptoManager.GenerateKeyPair();
            var encrypt = cryptoManager.Encrypt(publicKey, byteData);
            Console.WriteLine("Your encrypted data:");
            Console.WriteLine(encrypt.EncryptedData.FromByteArray());

            var decrypt = cryptoManager.Decrypt(privateKey, encrypt);
            // Console.WriteLine("and decrypted data:");
            // Console.WriteLine(decrypt.ToString());
        }
    }
}
