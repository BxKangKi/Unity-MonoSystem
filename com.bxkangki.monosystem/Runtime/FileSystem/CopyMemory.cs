using System.IO;
using System.Threading.Tasks;

namespace FileSystem
{
    public readonly struct CopyMemory
    {
        public static MemoryStream Copy(Stream stream)
        {
            MemoryStream ms = new MemoryStream();
            stream.CopyTo(ms);
            ms.Position = 0;
            return ms;
        }

        public static async Task<MemoryStream> CopyAsync(Stream stream)
        {
            MemoryStream ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            ms.Position = 0;
            return ms;
        }
    }
}