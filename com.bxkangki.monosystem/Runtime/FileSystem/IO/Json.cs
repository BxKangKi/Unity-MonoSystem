using System;
using System.IO;
using System.Threading.Tasks;


namespace FileSystem.IO
{
    /// <summary>
    /// Convert .NET objects to Json string and save to compressed or convert Json string to .NET objects load from decompressed file.
    /// </summary>
    public struct Json
    {
        public static void Write<T>(string path, T data, bool overwrite = true)
        {
            bool exist = Files.CheckFile(path);
            if (!overwrite && exist)
            {
                return;
            }
            if (!exist)
            {
                Files.CreateFile(path);
            }
            try
            {
                Bytes.WriteAllBytes(Compression.Compress(Serialization.Serialize(data)), path);
            }
            catch (Exception e)
            {
                LogSystem.Text(e.ToString());
            }
        }



        public static T Read<T>(string path)
        {
            if (!Files.CheckFile(path))
            {
                Files.CreateFile(path);
            }
            try
            {
                if (File.Exists(path))
                {
                    var bytes = Bytes.ReadAllBytes(path);
                    return Serialization.Deserialize<T>(Compression.Decompress(bytes));
                }
            }
            catch (Exception e)
            {
                LogSystem.Text(e.ToString());
            }
            return default;
        }


        public static async void WriteAsync<T>(string path, T data, bool overwrite = true)
        {
            bool exist = Files.CheckFile(path);
            if (!overwrite && exist)
            {
                return;
            }
            if (!exist)
            {
                Files.CreateFile(path);
            }
            try
            {
                Bytes.WriteAllBytesAsync(await Compression.CompressAsync(Serialization.Serialize(data)), path);
            }
            catch (Exception e)
            {
                LogSystem.Text(e.ToString());
            }
        }



        public static async Task<T> ReadAsync<T>(string path) where T : class
        {
            if (!Files.CheckFile(path))
            {
                Files.CreateFile(path);
            }
            try
            {
                if (File.Exists(path))
                {
                    var bytes = await Bytes.ReadAllBytesAsync(path);
                    return Serialization.Deserialize<T>(await Compression.DecompressAsync(bytes));
                }
            }
            catch (Exception e)
            {
                LogSystem.Text(e.ToString());
            }
            return null;
        }
    }
}