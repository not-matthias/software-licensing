using crypto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;

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
            var options = new JsonSerializerOptions
            {
                IgnoreNullValues = true
            };
            var publicKeyString = JsonSerializer.Serialize(_keys.publicKey, options);

            var packet = new Packet<string>
            {
                Data = publicKeyString,
                Checksum = _cryptoManager.GenerateHash(publicKeyString)
            };

            return Ok(packet);
        }

        // [FromBody] Packet<ProgramRequestData> requestPacket
        [HttpGet]
        [Route("validate")]
        public IActionResult ValidateLicense([FromQuery] string license)
        {
            if (string.IsNullOrEmpty(license))
            {
                return BadRequest();
            }

            if (!_licenses.ContainsKey(license))
            {
                return BadRequest();
            }

            //
            // Get the client public key
            //



            //
            // Read the file from disk
            //
            using FileStream fs = System.IO.File.Open(_licenses[license], FileMode.Open);
            using MemoryStream ms = new MemoryStream();

            byte[] buffer = new byte[1024];
            int read = 0;

            while ((read = fs.Read(buffer, 0, 1024)) > 0)
                ms.Write(buffer, 0, read);

            //
            // Encrypt the program
            //
            var encryptedProgram = _cryptoManager.Encrypt(_keys.publicKey.RSAParameters, ms.ToArray());

            //
            // Create the response packet
            //
            var packet = new Packet<byte[]>
            {
                Data = encryptedProgram,
                Checksum = _cryptoManager.GenerateHash(JsonSerializer.Serialize(encryptedProgram))
            };

            return Ok(packet);
        }
    }
}
