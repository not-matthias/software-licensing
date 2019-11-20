using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace utils
{
    public static class ByteConverter
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

        public static byte[] ToByteArray(this string data)
        {
            return Encoding.Default.GetBytes(data);
        }

        public static string FromByteArray(this byte[] data)
        {
            return Encoding.UTF8.GetString(data, 0, data.Length);
        }
    }
}
