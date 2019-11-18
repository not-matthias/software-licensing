using crypto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using utils;

namespace api.Controllers
{
    [ApiController]
    [Route("license")]
    public class LicenseController : ControllerBase
    {
        private readonly Dictionary<string, string> _licenses = new Dictionary<string, string>
        {
            {
                "AAAA-BBBB-CCCC-DDDD",
                "sample_software.dll"
            }
        };

        public static (RSAParametersSerializable publicKey, RSAParametersSerializable privateKey) _keys;

        private readonly ILogger<LicenseController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ICryptoManager _cryptoManager;

        static LicenseController()
        {
            using var rsa = new RSACng();
            _keys = (
                    publicKey: new RSAParametersSerializable(rsa.ExportParameters(false)),
                    privateKey: new RSAParametersSerializable(rsa.ExportParameters(true))
                );
        }

        public LicenseController(ILogger<LicenseController> logger, IConfiguration configuration, ICryptoManager cryptoManager)
        {
            _logger = logger;
            _configuration = configuration;
            _cryptoManager = cryptoManager;
        }

        [HttpGet]
        [Route("public_key")]
        public IActionResult GetPublicKey()
        {
            return Ok(new Packet<RSAParametersSerializable>(_keys.publicKey));
        }

        [HttpPost]
        [Route("validate")]
        public async Task<IActionResult> ValidateLicenseAsync([FromBody] EncryptedPacket<CryptoData> requestPacket)
        {
            //
            // Compare the checksum
            //
            if (!requestPacket.IsValid())
            {
                return BadRequest();
            }

            //
            // Decrypt the packet
            //
            var data = await requestPacket.DecryptAsync<ProgramRequestData>(_keys.privateKey.RSAParameters);

            //
            // Validate the license
            //
            if (string.IsNullOrEmpty(data.LicenseKey) || !_licenses.ContainsKey(data.LicenseKey))
            {
                return BadRequest();
            }

            //
            // Read the file from disk
            //
            using FileStream fs = System.IO.File.Open(_licenses[data.LicenseKey], FileMode.Open);
            using MemoryStream ms = new MemoryStream();

            byte[] buffer = new byte[1024];
            int read = 0;

            while ((read = fs.Read(buffer, 0, 1024)) > 0)
                ms.Write(buffer, 0, read);

            //
            // Create the response packet
            //
            var packet = new Packet<byte[]>(ms.ToArray());
            return Ok(await packet.EncryptAsync(data.PublicKey.RSAParameters));
        }
    }
}
