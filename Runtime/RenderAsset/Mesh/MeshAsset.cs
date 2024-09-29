using UnityEngine;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace MonoSystem
{
    public struct MeshAsset
    {
        public static Mesh Load(Stream stream)
        {
            using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
            {
                var mesh = new Mesh();
                mesh.SetVertices(BinarySystem.Vector3.ReadArray(reader));
                mesh.SetUVs(0, BinarySystem.Vector2.ReadArray(reader));
                mesh.SetUVs(1, BinarySystem.Vector2.ReadArray(reader));
                mesh.SetUVs(2, BinarySystem.Vector2.ReadArray(reader));
                mesh.SetUVs(3, BinarySystem.Vector2.ReadArray(reader));
                mesh.SetUVs(4, BinarySystem.Vector2.ReadArray(reader));
                mesh.SetUVs(5, BinarySystem.Vector2.ReadArray(reader));
                mesh.SetUVs(6, BinarySystem.Vector2.ReadArray(reader));
                mesh.SetUVs(7, BinarySystem.Vector2.ReadArray(reader));
                mesh.SetColors(BinarySystem.Color32.ReadArray(reader));
                int count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    mesh.SetTriangles(BinarySystem.Int32.ReadArray(reader), i);
                }
                count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    mesh = BlendShapeAsset.Load(reader, mesh);
                }
                mesh.boneWeights = BinarySystem.BoneWeight.ReadArray(reader);
                mesh.bindposes = BinarySystem.Matrix4x4.ReadArray(reader);
                mesh.RecalculateNormals();
                mesh.RecalculateTangents();
                mesh.RecalculateBounds();
                mesh.Optimize();
                return mesh;
            }
        }

        public static void Save(Stream stream, Mesh mesh)
        {
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, false))
            {
                BinarySystem.Vector3.Write(mesh.vertices, writer);
                BinarySystem.Vector2.Write(mesh.uv, writer);
                BinarySystem.Vector2.Write(mesh.uv2, writer);
                BinarySystem.Vector2.Write(mesh.uv3, writer);
                BinarySystem.Vector2.Write(mesh.uv4, writer);
                BinarySystem.Vector2.Write(mesh.uv5, writer);
                BinarySystem.Vector2.Write(mesh.uv6, writer);
                BinarySystem.Vector2.Write(mesh.uv7, writer);
                BinarySystem.Vector2.Write(mesh.uv8, writer);
                BinarySystem.Color32.Write(mesh.colors32, writer);
                writer.Write(mesh.subMeshCount);
                for (int i = 0; i < mesh.subMeshCount; i++)
                {
                    BinarySystem.Int32.Write(mesh.GetTriangles(i), writer);
                }
                writer.Write(mesh.blendShapeCount);
                for (int i = 0; i < mesh.blendShapeCount; i++)
                {
                    BlendShapeAsset.Save(writer, mesh, i);
                }
                BinarySystem.BoneWeight.Write(mesh.boneWeights, writer);
                BinarySystem.Matrix4x4.Write(mesh.bindposes, writer);
            }
        }
    }
}
