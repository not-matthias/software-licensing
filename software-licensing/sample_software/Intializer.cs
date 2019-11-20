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
            var userInput = Console.ReadLine();

            var byteData = userInput.ToByteArray();

            var (publicKey, privateKey) = cryptoManager.GenerateKeyPair();
            var encrypt = cryptoManager.Encrypt(publicKey, byteData);
            Console.WriteLine(encrypt.EncryptedData.FromByteArray());
        }
    }
}
