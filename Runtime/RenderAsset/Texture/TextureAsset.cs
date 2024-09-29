using System.IO;
using System.Text;
using UnityEngine;

namespace MonoSystem
{
    public struct TextureAsset
    {

        public static void Save(Stream stream, Texture tex)
        {
            Texture2D tex2D = new Texture2D(tex.width, tex.height, TextureFormat.RGBA32, 1, !tex.isDataSRGB);
            tex2D.SetPixels32(CopyTexture(tex).GetPixels32());
            tex2D.Apply();
            var data = tex2D.EncodeToPNG();
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, false))
            {
                writer.Write(data);
            }
        }


        public static void Save(Stream stream, Texture2DArray array)
        {
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, false))
            {
                for (int i = 0; i < array.depth; i++)
                {
                    var data = CopyTexture(array, i).EncodeToPNG();
                    writer.Write(data.Length);
                    writer.Write(data);
                }
            }
        }


        public static Texture2DArray Load(Stream stream, int width, int height, int depth, bool linear)
        {
            using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
            {
                var array = new Texture2DArray(width, height, depth, TextureFormat.RGBA32, -1, linear, true);
                for (int i = 0; i < depth; i++)
                {
                    int length = reader.ReadInt32();
                    var data = reader.ReadBytes(length);
                    var tex2D = new Texture2D(width, height, TextureFormat.RGBA32, -1, linear);
                    tex2D.LoadImage(data);
                    tex2D.Compress(false);
                    tex2D.Apply();
                    array.SetPixels32(tex2D.GetPixels32(), i);
                }
                return array;
            }
        }


        public static Texture2D Load(Stream stream, int width, int height, bool linear)
        {
            using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
            {
                int length = (int)reader.BaseStream.Length;
                var data = reader.ReadBytes(length);
                var tex2D = new Texture2D(width, height, TextureFormat.RGBA32, -1, linear);
                tex2D.LoadImage(data);
                tex2D.Compress(false);
                tex2D.Apply();
                return tex2D;
            }
        }


        private static Texture2D CopyTexture(Texture tex)
        {
            Texture2D scr = tex as Texture2D;
            Texture2D dst = new Texture2D(tex.width, tex.height, scr.format, tex.mipmapCount, !tex.isDataSRGB);
            dst.LoadRawTextureData(scr.GetRawTextureData());
            dst.Apply();
            return dst;
        }

        private static Texture2D CopyTexture(Texture2DArray array, int i)
        {
            Texture2D tex2D = new Texture2D(array.width, array.height, TextureFormat.RGBA32, -1, array.isDataSRGB);
            tex2D.SetPixels32(array.GetPixels32(i));
            tex2D.Apply();
            return tex2D;
        }
    }
}