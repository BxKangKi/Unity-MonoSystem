using System;
using System.IO;
using System.Threading.Tasks;

namespace FileSystem.IO
{
    public struct Bytes
    {
        public static void WriteAllBytes(byte[] bytes, string path)
        {
            try
            {
                File.WriteAllBytes(path, bytes);
            }
            catch (Exception e)
            {
                LogSystem.Text(e.ToString());
            }
        }


        public static void WriteObjectToBytes<T>(BinaryWriter writer, T data)
        {
            byte[] bytes = Serialization.Serialize<T>(data);
            writer.Write(bytes.Length);
            writer.Write(bytes);
        }

        public static async void WriteAllBytesAsync(byte[] bytes, string path)
        {
            try
            {
                await File.WriteAllBytesAsync(path, bytes);
            }
            catch (Exception e)
            {
                LogSystem.Text(e.ToString());
            }
        }



        public static T ReadObjectFromBytes<T>(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            byte[] bytes = reader.ReadBytes(count);
            return Serialization.Deserialize<T>(bytes);
        }



        public static byte[] ReadAllBytes(string path)
        {
            try
            {
                return File.ReadAllBytes(path);
            }
            catch (Exception e)
            {
                LogSystem.Text(e.ToString());
                return null;
            }
        }


        public static async Task<byte[]> ReadAllBytesAsync(string path)
        {
            try
            {
                var result = await File.ReadAllBytesAsync(path);
                return result;
            }
            catch (Exception e)
            {
                LogSystem.Text(e.ToString());

                return null;
            }
        }
    }
}