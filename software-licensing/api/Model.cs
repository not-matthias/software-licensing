namespace api
{
    public class Packet<T>
    {
        public T Data { get; set; }
        public string Checksum { get; set; }
    }

    public class ProgramRequestData
    {
        public string PublicKey { get; set; }
        public string LicenseKey { get; set; }
    }
}
