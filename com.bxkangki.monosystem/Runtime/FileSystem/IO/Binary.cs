
using System.IO;
using Unity.Mathematics;

namespace FileSystem.IO
{
    public struct Binary
    {
        #region Numbers
        public struct Int32
        {
            public static int[] ReadArray(BinaryReader reader)
            {
                int count = reader.ReadInt32();
                var result = new int[count];
                for (int i = 0; i < count; i++)
                {
                    result[i] = reader.ReadInt32();
                }
                return result;
            }

            public static void Write(int[] array, BinaryWriter writer)
            {
                int count = array.Length;
                writer.Write(count);
                for (int i = 0; i < count; i++)
                {
                    writer.Write(array[i]);
                }
            }
        }
        #endregion


        #region Unity.Mathematics
        public struct Int32x4
        {
            public static int4 Read(BinaryReader reader)
            {
                int x = reader.ReadInt32();
                int y = reader.ReadInt32();
                int z = reader.ReadInt32();
                int w = reader.ReadInt32();
                return new int4(x, y, z, w);
            }
            public static int4[] ReadArray(BinaryReader reader)
            {
                int count = reader.ReadInt32();
                var result = new int4[count];
                for (int i = 0; i < count; i++)
                {
                    result[i] = Read(reader);
                }
                return result;
            }

            public static void Write(int4 int32x4, BinaryWriter writer)
            {
                writer.Write(int32x4.x);
                writer.Write(int32x4.y);
                writer.Write(int32x4.z);
                writer.Write(int32x4.w);
            }

            public static void Write(int4[] array, BinaryWriter writer)
            {
                int count = array.Length;
                writer.Write(count);
                for (int i = 0; i < count; i++)
                {
                    Write(array[i], writer);
                }
            }
        }
        #endregion


        #region Vector
        public struct Vector2
        {
            public static UnityEngine.Vector2 Read(BinaryReader reader)
            {
                float x = reader.ReadSingle();
                float y = reader.ReadSingle();
                return new UnityEngine.Vector2(x, y);
            }

            public static UnityEngine.Vector2[] ReadArray(BinaryReader reader)
            {
                int count = reader.ReadInt32();
                var result = new UnityEngine.Vector2[count];
                for (int i = 0; i < count; i++)
                {
                    result[i] = Read(reader);
                }
                return result;
            }

            public static void Write(UnityEngine.Vector2 vector, BinaryWriter writer)
            {
                writer.Write(vector.x);
                writer.Write(vector.y);
            }


            public static void Write(UnityEngine.Vector2[] array, BinaryWriter writer)
            {
                int count = array.Length;
                writer.Write(count);
                for (int i = 0; i < count; i++)
                {
                    Write(array[i], writer);
                }
            }
        }


        public struct Vector3
        {
            public static UnityEngine.Vector3 Read(BinaryReader reader)
            {
                float x = reader.ReadSingle();
                float y = reader.ReadSingle();
                float z = reader.ReadSingle();
                return new UnityEngine.Vector3(x, y, z);
            }

            public static UnityEngine.Vector3[] ReadArray(BinaryReader reader)
            {
                int count = reader.ReadInt32();
                var result = new UnityEngine.Vector3[count];
                for (int i = 0; i < count; i++)
                {
                    result[i] = Read(reader);
                }
                return result;
            }

            public static void Write(UnityEngine.Vector3 vector, BinaryWriter writer)
            {
                writer.Write(vector.x);
                writer.Write(vector.y);
                writer.Write(vector.z);
            }

            public static void Write(UnityEngine.Vector3[] array, BinaryWriter writer)
            {
                int count = array.Length;
                writer.Write(count);
                for (int i = 0; i < count; i++)
                {
                    Write(array[i], writer);
                }
            }
        }


        public struct Vector4
        {
            public static UnityEngine.Vector4 Read(BinaryReader reader)
            {
                float x = reader.ReadSingle();
                float y = reader.ReadSingle();
                float z = reader.ReadSingle();
                float w = reader.ReadSingle();
                return new UnityEngine.Vector4(x, y, z, w);
            }

            public static UnityEngine.Vector4[] ReadArray(BinaryReader reader)
            {
                int count = reader.ReadInt32();
                var result = new UnityEngine.Vector4[count];
                for (int i = 0; i < count; i++)
                {
                    result[i] = Read(reader);
                }
                return result;
            }

            public static void Write(UnityEngine.Vector4 vector, BinaryWriter writer)
            {
                writer.Write(vector.x);
                writer.Write(vector.y);
                writer.Write(vector.z);
                writer.Write(vector.w);
            }

            public static void Write(UnityEngine.Vector4[] array, BinaryWriter writer)
            {
                int count = array.Length;
                writer.Write(count);
                for (int i = 0; i < count; i++)
                {
                    Write(array[i], writer);
                }
            }
        }
        #endregion


        #region Color32
        public struct Color32
        {
            public static UnityEngine.Color32 Read(BinaryReader reader)
            {
                byte r = reader.ReadByte();
                byte g = reader.ReadByte();
                byte b = reader.ReadByte();
                byte a = reader.ReadByte();
                return new UnityEngine.Color32(r, g, b, a);
            }

            public static UnityEngine.Color32[] ReadArray(BinaryReader reader)
            {
                int count = reader.ReadInt32();
                var result = new UnityEngine.Color32[count];
                for (int i = 0; i < count; i++)
                {
                    result[i] = Read(reader);
                }
                return result;
            }


            public static void Write(UnityEngine.Color32 color32, BinaryWriter writer)
            {
                writer.Write(color32.r);
                writer.Write(color32.g);
                writer.Write(color32.b);
                writer.Write(color32.a);
            }

            public static void Write(UnityEngine.Color32[] array, BinaryWriter writer)
            {
                int count = array.Length;
                writer.Write(count);
                for (int i = 0; i < count; i++)
                {
                    Write(array[i], writer);
                }
            }
        }
        #endregion


        #region Matrix4x4
        public struct Matrix4x4
        {
            public static UnityEngine.Matrix4x4 Read(BinaryReader reader)
            {
                return new UnityEngine.Matrix4x4
                {
                    m00 = reader.ReadSingle(),
                    m01 = reader.ReadSingle(),
                    m02 = reader.ReadSingle(),
                    m03 = reader.ReadSingle(),
                    m10 = reader.ReadSingle(),
                    m11 = reader.ReadSingle(),
                    m12 = reader.ReadSingle(),
                    m13 = reader.ReadSingle(),
                    m20 = reader.ReadSingle(),
                    m21 = reader.ReadSingle(),
                    m22 = reader.ReadSingle(),
                    m23 = reader.ReadSingle(),
                    m30 = reader.ReadSingle(),
                    m31 = reader.ReadSingle(),
                    m32 = reader.ReadSingle(),
                    m33 = reader.ReadSingle()
                };
            }

            public static UnityEngine.Matrix4x4[] ReadArray(BinaryReader reader)
            {
                int count = reader.ReadInt32();
                var result = new UnityEngine.Matrix4x4[count];
                for (int i = 0; i < count; i++)
                {
                    result[i] = Read(reader);
                }
                return result;
            }


            public static void Write(UnityEngine.Matrix4x4 matrix, BinaryWriter writer)
            {
                writer.Write(matrix.m00);
                writer.Write(matrix.m01);
                writer.Write(matrix.m02);
                writer.Write(matrix.m03);
                writer.Write(matrix.m10);
                writer.Write(matrix.m11);
                writer.Write(matrix.m12);
                writer.Write(matrix.m13);
                writer.Write(matrix.m20);
                writer.Write(matrix.m21);
                writer.Write(matrix.m22);
                writer.Write(matrix.m23);
                writer.Write(matrix.m30);
                writer.Write(matrix.m31);
                writer.Write(matrix.m32);
                writer.Write(matrix.m33);
            }

            public static void Write(UnityEngine.Matrix4x4[] array, BinaryWriter writer)
            {
                int count = array.Length;
                writer.Write(count);
                for (int i = 0; i < count; i++)
                {
                    Write(array[i], writer);
                }
            }
        }
        #endregion

        #region BoneWeight
        public struct BoneWeight
        {
            public static UnityEngine.BoneWeight Read(BinaryReader reader)
            {
                return new UnityEngine.BoneWeight
                {
                    weight0 = reader.ReadSingle(),
                    weight1 = reader.ReadSingle(),
                    weight2 = reader.ReadSingle(),
                    weight3 = reader.ReadSingle(),
                    boneIndex0 = reader.ReadInt32(),
                    boneIndex1 = reader.ReadInt32(),
                    boneIndex2 = reader.ReadInt32(),
                    boneIndex3 = reader.ReadInt32()
                };
            }

            public static UnityEngine.BoneWeight[] ReadArray(BinaryReader reader)
            {
                int count = reader.ReadInt32();
                var result = new UnityEngine.BoneWeight[count];
                for (int i = 0; i < count; i++)
                {
                    result[i] = Read(reader);
                }
                return result;
            }


            public static void Write(UnityEngine.BoneWeight boneWeight, BinaryWriter writer)
            {
                writer.Write(boneWeight.weight0);
                writer.Write(boneWeight.weight1);
                writer.Write(boneWeight.weight2);
                writer.Write(boneWeight.weight3);
                writer.Write(boneWeight.boneIndex0);
                writer.Write(boneWeight.boneIndex1);
                writer.Write(boneWeight.boneIndex2);
                writer.Write(boneWeight.boneIndex3);
            }

            public static void Write(UnityEngine.BoneWeight[] array, BinaryWriter writer)
            {
                int count = array.Length;
                writer.Write(count);
                for (int i = 0; i < count; i++)
                {
                    Write(array[i], writer);
                }
            }
        }
        #endregion
    }
}