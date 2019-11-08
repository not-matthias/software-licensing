using crypto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
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
                "temp.dll"
            }
        };

        private readonly ILogger<LicenseController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ICryptoManager _cryptoManager;

        public LicenseController(ILogger<LicenseController> logger, IConfiguration configuration, ICryptoManager cryptoManager)
        {
            _logger = logger;
            _configuration = configuration;
            _cryptoManager = cryptoManager;
        }

        [HttpGet]
        [Route("get")]
        public IActionResult Get([FromQuery] string license)
        {
            if (string.IsNullOrEmpty(license))
            {
                return BadRequest();
            }

            if (!_licenses.ContainsKey(license))
            {
                return BadRequest();
            }

            Packet<ResponseData> packet = new Packet<ResponseData>();

            var programData = new byte[100];
            packet.Data = new ResponseData { ProgramData = programData };
            packet.Checksum = _cryptoManager.GenerateHash(JsonSerializer.Serialize(packet.Data));

            return Ok(packet);
        }
    }
}
