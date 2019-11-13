using api;
using crypto;
using System;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using utils;

namespace console
{
    class Program
    {
        public static HttpClient client = new HttpClient() { BaseAddress = new Uri("http://localhost:53696") };
        public static CryptoManager cryptoManager = new CryptoManager();

        static async Task Main(string[] args)
        {
            //   var publicKey = await GetPublicKey();
            var program = await GetProgram(null, "AAAA-BBBB-CCCC-DDDD");

            LaunchProgram(Assembly.Load(program));
        }

        public static T Deserialize<T>(string jsonContent)
        {
            return JsonSerializer.Deserialize<T>(jsonContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public static async Task<RSAParametersSerializable> GetPublicKey()
        {
            var response = await client.GetStringAsync("/license/public_key");
            var packet = Deserialize<Packet<RSAParametersSerializable>>(response);

            if (!packet.IsValid())
            {
                Console.WriteLine("Packet is not valid.");
                throw new InvalidOperationException();
            }

            return packet.Data;
        }

        public static async Task<byte[]> GetProgram(RSAParametersSerializable publicKey, string licenseKey)
        {
            var keys = cryptoManager.GenerateKeyPair();
            var packet = new Packet<ProgramRequestData>(new ProgramRequestData
            {
                LicenseKey = licenseKey,
                PublicKey = new RSAParametersSerializable(keys.publicKey)
            });

            var encryptedPacket = await packet.EncryptAsync(keys.publicKey);
            var decryptedPacket = await encryptedPacket.DecryptAsync<ProgramRequestData>(keys.privateKey);

            Console.WriteLine("Finished the encryption.");

            //var content = new StringContent(JsonSerializer.Serialize(), Encoding.UTF8, "application/json");
            //var response = await client.PostAsync("/program", content);
            // Get the program from the server
            return new byte[100];
        }

        public static async void LaunchProgram(Assembly assembly)
        {
            // TODO: Launch it with the IProgramLoader.cs
        }
    }
}
