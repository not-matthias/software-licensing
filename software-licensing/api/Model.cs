namespace api
{
    public class Packet<T>
    {
        public T Data { get; set; }
        public string Checksum { get; set; }
    }

    public class ProgramRequestData
    {
        public RSAParametersSerializable PublicKey { get; set; }
        public string LicenseKey { get; set; }
    }
}
