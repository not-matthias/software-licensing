using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using utils;

namespace console
{
    class Program
    {
        public static HttpClient client = new HttpClient() { BaseAddress = new Uri("http://localhost:5000") };

        static async Task Main(string[] args)
        {
            await GetPublicKey();
        }

        public static T Deserialize<T>(string jsonContent)
        {
            return JsonSerializer.Deserialize<T>(jsonContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public static async Task GetPublicKey()
        {
            try
            {
                var response = await client.GetStringAsync("/license/public_key");
                var packet = Deserialize<Packet<RSAParametersSerializable>>(response);

                Console.WriteLine(packet.IsValid());
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }
    }
}
