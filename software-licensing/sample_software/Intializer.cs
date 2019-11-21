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
            Console.WriteLine();

            var byteData = userInput.ToByteArray();
            var (publicKey, privateKey) = cryptoManager.GenerateKeyPair();

            //
            // Encrypt
            //
            var encrypt = cryptoManager.Encrypt(publicKey, byteData);
            Console.WriteLine("Your encrypted data:");
            Console.WriteLine(encrypt.EncryptedData.FromByteArray() + "\n");

            //
            // Decrypt
            //
            var decrypt = cryptoManager.Decrypt(privateKey, encrypt);
            Console.WriteLine("Your decrypted data:");
            Console.WriteLine(decrypt.FromByteArray() + "\n");

            //
            // Hash
            //
            var dataHash = cryptoManager.GenerateHash(decrypt);
            Console.WriteLine("Your data hash:");
            Console.WriteLine(dataHash);
        }
    }
}
