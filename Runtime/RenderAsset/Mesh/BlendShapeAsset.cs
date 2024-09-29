using System.IO;
using UnityEngine;

namespace MonoSystem
{
    internal struct BlendShapeAsset
    {
        public static void Save(BinaryWriter writer, Mesh mesh, int index)
        {
            int frameCount = mesh.GetBlendShapeFrameCount(index);
            string name = mesh.GetBlendShapeName(index);
            writer.Write(frameCount);
            writer.Write(name);
            for (int i = 0; i < frameCount; i++)
            {
                var deltaVertices = new Vector3[mesh.vertexCount];
                var deltaNormals = new Vector3[mesh.vertexCount];
                var deltaTangents = new Vector3[mesh.vertexCount];
                float frameWeight = mesh.GetBlendShapeFrameWeight(index, i);
                mesh.GetBlendShapeFrameVertices(index, i, deltaVertices, deltaNormals, deltaTangents);
                SaveBlendShapeFrame(writer, frameWeight, deltaVertices, deltaNormals, deltaTangents);
            }
        }

        private const int Multiplier = 100000;
        private const float Threshold = 1f / Multiplier;

        public static void SaveBlendShapeFrame(BinaryWriter writer, float frameWeight, Vector3[] deltaVertices, Vector3[] deltaNormals, Vector3[] deltaTangents)
        {
            writer.Write(frameWeight);
            BinarySystem.Vector3.Write(deltaVertices, writer);
            BinarySystem.Vector3.Write(deltaNormals, writer);
            BinarySystem.Vector3.Write(deltaTangents, writer);
        }

        public static Mesh LoadBlendShapeFrame(Mesh mesh, BinaryReader reader, string shapeName)
        {
            float weight = reader.ReadSingle();
            mesh.AddBlendShapeFrame(shapeName, weight, BinarySystem.Vector3.ReadArray(reader), BinarySystem.Vector3.ReadArray(reader), BinarySystem.Vector3.ReadArray(reader));
            return mesh;
        }

        public static Mesh Load(BinaryReader reader, Mesh mesh)
        {
            int frameCount = reader.ReadInt32();
            string name = reader.ReadString();
            for (int i = 0; i < frameCount; i++)
            {
                mesh = LoadBlendShapeFrame(mesh, reader, name);
            }
            return mesh;
        }
    }
}