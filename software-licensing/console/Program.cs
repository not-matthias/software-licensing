using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using utils;

namespace console
{
    class Program
    {
        public static HttpClient client = new HttpClient() { BaseAddress = new Uri("http://localhost:53696") };
        static async Task Main(string[] args)
        {
            _ = await GetPublicKey();
        }

        public static async Task<string> GetPublicKey()
        {
            // DecryptedPacket<string>try 
            try
            {
                var response = await client.GetStringAsync("/license/public_key");

                var packet = JsonSerializer.Deserialize<DecryptedPacket<RSAParametersSerializable>>(response);


                Console.WriteLine("Testasdf");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }

            Console.WriteLine("Hey!");

            return "You suck";
        }
    }
}
