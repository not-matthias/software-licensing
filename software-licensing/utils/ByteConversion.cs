using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace utils
{
    public static class ByteConversion
    {
        public static byte[] ToByteArray<V>(this V obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            using MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }

        public static V FromByteArray<V>(this byte[] data)
        {
            if (data == null)
                return default;

            BinaryFormatter bf = new BinaryFormatter();
            using MemoryStream ms = new MemoryStream(data);
            object obj = bf.Deserialize(ms);
            return (V)obj;
        }
    }
}
