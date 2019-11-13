using System;

namespace api
{
    [Serializable]
    public class ProgramRequestData
    {
        public RSAParametersSerializable PublicKey { get; set; }
        public string LicenseKey { get; set; }
    }
}
