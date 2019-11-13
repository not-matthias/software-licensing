using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;

namespace utils
{
    public abstract class Packet<DT>
    {
        public DT Data { get; set; }
        public string Checksum { get; set; }

        public bool IsValid()
        {
            return JsonSerializer.Serialize(Data) == Checksum;
        }

        public static byte[] ToByteArray<T>(T obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            using MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }

        public static T FromByteArray<T>(byte[] data)
        {
            if (data == null)
                return default;

            BinaryFormatter bf = new BinaryFormatter();
            using MemoryStream ms = new MemoryStream(data);
            object obj = bf.Deserialize(ms);
            return (T)obj;
        }
    }
}
