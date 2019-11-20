using crypto;
using System;

namespace sample_software
{
    public class Intializer
    {
        public static CryptoManager cryptoManager = new CryptoManager();

        public static void Init()
        {
            var userInput = Console.ReadLine();
            Console.WriteLine($"Hello: {userInput}");
            Console.WriteLine("Hello World!");
        }
    }
}
