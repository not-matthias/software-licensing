using api;
using crypto;
using System;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using utils;

namespace console
{
    class Program
    {
        public static HttpClient client = new HttpClient() { BaseAddress = new Uri("http://localhost:5000") };
        public static CryptoManager cryptoManager = new CryptoManager();
        public static ProgramLoader programLoader = new ProgramLoader();

        static async Task Main(string[] args)
        {
            var publicKey = await GetPublicKey();
            var program = await GetProgram(publicKey, "AAAA-BBBB-CCCC-DDDD");

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

            //
            // Create the request packet
            //
            var packet = new Packet<ProgramRequestData>(new ProgramRequestData
            {
                LicenseKey = licenseKey,
                PublicKey = new RSAParametersSerializable(keys.publicKey)
            });
            var encryptedPacket = await packet.EncryptAsync(publicKey.RSAParameters);

            //
            // Request the program from the server
            //
            var content = new StringContent(JsonSerializer.Serialize(encryptedPacket), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/license/validate", content);

            //
            // Deserialize and decrypt the packet
            //
            var encrypted = Deserialize<EncryptedPacket<CryptoData>>(await response.Content.ReadAsStringAsync());
            var decypted = await encrypted.DecryptAsync<byte[]>(keys.privateKey);

            return decypted;
        }

        public static void LaunchProgram(Assembly assembly)
        {
            programLoader.CallFunction(assembly, "sample_software.Intializer", "Init");
        }
    }
}
