using System.IO;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace MonoSystem
{
    public struct BinarySystem
    {
        public static FixedString64Bytes ReadFixedString64BytesFromBytes(NativeArray<byte> array, int startIndex)
        {
            // Burst-friendly 방식으로 포인터 참조
            unsafe
            {
                byte* bytePtr = (byte*)array.GetUnsafeReadOnlyPtr() + startIndex;
                return *(FixedString64Bytes*)bytePtr;
            }
        }

        public static float ReadFloatFromBytes(NativeArray<byte> array, int startIndex)
        {
            // Burst-friendly 방식으로 포인터 참조
            unsafe
            {
                byte* bytePtr = (byte*)array.GetUnsafeReadOnlyPtr() + startIndex;
                return *(float*)bytePtr;
            }
        }

        public static int ReadIntFromBytes(NativeArray<byte> array, int startIndex)
        {
            // Burst-friendly 방식으로 포인터 참조
            unsafe
            {
                byte* bytePtr = (byte*)array.GetUnsafeReadOnlyPtr() + startIndex;
                return *(int*)bytePtr;
            }
        }

        #region String
        public struct String
        {
            // Burst 컴파일러를 사용한 Job 정의
            [BurstCompile]
            private struct ConvertByteToFixedString512BytesJob : IJobParallelFor
            {
                [ReadOnly] public NativeArray<byte> ByteArray;
                public NativeArray<FixedString512Bytes> StringArray;
                public void Execute(int index)
                {
                    int byteIndex = index * 2 * sizeof(float);
                    // ByteArray에서 각 요소를 직접 변환하여 Vector3를 만듭니다.
                    FixedString512Bytes str = ReadFixedString64BytesFromBytes(ByteArray, byteIndex);
                    StringArray[index] = str;
                }
            }

            public static string[] ReadArray(BinaryReader reader)
            {
                int count = reader.ReadInt32();
                using var bytes = new NativeArray<byte>(reader.ReadBytes(count * sizeof(float) * 2), Allocator.TempJob);
                using var str = new NativeArray<FixedString512Bytes>(count, Allocator.TempJob);
                var job = new ConvertByteToFixedString512BytesJob()
                {
                    ByteArray = bytes,
                    StringArray = str
                };
                var handle = job.Schedule(count, -1);
                handle.Complete();
                string[] array = new string[str.Length];
                for (int i = 0; i < str.Length; i++)
                {
                    array[i] = str[i].ToString();
                }
                return array;
            }

            public static void Write(string str, BinaryWriter writer)
            {
                var fixedstr = new FixedString512Bytes(str);
                int size = UnsafeUtility.SizeOf<FixedString512Bytes>();
                NativeArray<byte> bytes = new NativeArray<byte>(size, Allocator.Temp);
                unsafe
                {
                    void* ptr = UnsafeUtility.AddressOf(ref fixedstr);
                    UnsafeUtility.MemCpy(bytes.GetUnsafePtr(), ptr, size);
                }
                writer.Write(bytes);
            }


            public static void Write(string[] array, BinaryWriter writer)
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

        #region Numbers
        public struct Int32
        {
            // Burst 컴파일러를 사용한 Job 정의
            [BurstCompile]
            private struct ConvertByteToIntJob : IJobParallelFor
            {
                [ReadOnly] public NativeArray<byte> ByteArray;
                public NativeArray<int> IntArray;

                public void Execute(int index)
                {
                    int byteIndex = index * sizeof(int);
                    IntArray[index] = ReadIntFromBytes(ByteArray, byteIndex);
                }
            }

            public static int[] ReadArray(BinaryReader reader)
            {
                int count = reader.ReadInt32();
                using var bytes = new NativeArray<byte>(reader.ReadBytes(count * sizeof(int)), Allocator.TempJob);
                using var array = new NativeArray<int>(count, Allocator.TempJob);
                var job = new ConvertByteToIntJob()
                {
                    ByteArray = bytes,
                    IntArray = array
                };
                var handle = job.Schedule(count, -1);
                handle.Complete();
                return array.ToArray();
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


        public struct Float
        {
            // Burst 컴파일러를 사용한 Job 정의
            [BurstCompile]
            private struct ConvertByteToFloatJob : IJobParallelFor
            {
                [ReadOnly] public NativeArray<byte> ByteArray;
                public NativeArray<float> FloatArray;

                public void Execute(int index)
                {
                    int byteIndex = index * sizeof(float);
                    FloatArray[index] = ReadFloatFromBytes(ByteArray, byteIndex);
                }
            }

            public static float[] ReadArray(BinaryReader reader)
            {
                int count = reader.ReadInt32();
                using var bytes = new NativeArray<byte>(reader.ReadBytes(count * sizeof(int)), Allocator.TempJob);
                using var array = new NativeArray<float>(count, Allocator.TempJob);
                var job = new ConvertByteToFloatJob()
                {
                    ByteArray = bytes,
                    FloatArray = array
                };
                var handle = job.Schedule(count, -1);
                handle.Complete();
                return array.ToArray();
            }

            public static void Write(float[] array, BinaryWriter writer)
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
            // Burst 컴파일러를 사용한 Job 정의
            [BurstCompile]
            private struct ConvertByteToVector2Job : IJobParallelFor
            {
                [ReadOnly] public NativeArray<byte> ByteArray;
                public NativeArray<UnityEngine.Vector2> VectorArray;
                public void Execute(int index)
                {
                    int byteIndex = index * 2 * sizeof(float);

                    // ByteArray에서 각 요소를 직접 변환하여 Vector3를 만듭니다.
                    UnityEngine.Vector2 vector = new UnityEngine.Vector2(
                        ReadFloatFromBytes(ByteArray, byteIndex),
                        ReadFloatFromBytes(ByteArray, byteIndex + sizeof(float))
                    );
                    VectorArray[index] = vector;
                }
            }

            public static UnityEngine.Vector2 Read(BinaryReader reader)
            {
                float x = reader.ReadSingle();
                float y = reader.ReadSingle();
                return new UnityEngine.Vector2(x, y);
            }

            public static UnityEngine.Vector2[] ReadArray(BinaryReader reader)
            {
                int count = reader.ReadInt32();
                using var bytes = new NativeArray<byte>(reader.ReadBytes(count * sizeof(float) * 2), Allocator.TempJob);
                using var vector = new NativeArray<UnityEngine.Vector2>(count, Allocator.TempJob);
                var job = new ConvertByteToVector2Job()
                {
                    ByteArray = bytes,
                    VectorArray = vector
                };
                var handle = job.Schedule(count, -1);
                handle.Complete();
                return vector.ToArray();
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
            // Burst 컴파일러를 사용한 Job 정의
            [BurstCompile]
            private struct ConvertByteToVector3Job : IJobParallelFor
            {
                [ReadOnly] public NativeArray<byte> ByteArray;
                public NativeArray<UnityEngine.Vector3> VectorArray;

                public void Execute(int index)
                {
                    int byteIndex = index * 3 * sizeof(float);

                    // ByteArray에서 각 요소를 직접 변환하여 Vector3를 만듭니다.
                    UnityEngine.Vector3 vector = new UnityEngine.Vector3(
                        ReadFloatFromBytes(ByteArray, byteIndex),
                        ReadFloatFromBytes(ByteArray, byteIndex + sizeof(float)),
                        ReadFloatFromBytes(ByteArray, byteIndex + 2 * sizeof(float))
                    );
                    VectorArray[index] = vector;
                }
            }

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
                using var bytes = new NativeArray<byte>(reader.ReadBytes(count * sizeof(float) * 3), Allocator.TempJob);
                using var vector = new NativeArray<UnityEngine.Vector3>(count, Allocator.TempJob);
                var job = new ConvertByteToVector3Job()
                {
                    ByteArray = bytes,
                    VectorArray = vector
                };
                var handle = job.Schedule(count, -1);
                handle.Complete();
                return vector.ToArray();
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
            // Burst 컴파일러를 사용한 Job 정의
            [BurstCompile]
            private struct ConvertByteToVector4Job : IJobParallelFor
            {
                [ReadOnly] public NativeArray<byte> ByteArray;
                public NativeArray<UnityEngine.Vector4> VectorArray;

                public void Execute(int index)
                {
                    int byteIndex = index * 4 * sizeof(float);

                    // ByteArray에서 각 요소를 직접 변환하여 Vector3를 만듭니다.
                    UnityEngine.Vector4 vector = new UnityEngine.Vector4(
                        ReadFloatFromBytes(ByteArray, byteIndex),
                        ReadFloatFromBytes(ByteArray, byteIndex + sizeof(float)),
                        ReadFloatFromBytes(ByteArray, byteIndex + 2 * sizeof(float)),
                        ReadFloatFromBytes(ByteArray, byteIndex + 3 * sizeof(float))
                    );
                    VectorArray[index] = vector;
                }
            }

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
                using var bytes = new NativeArray<byte>(reader.ReadBytes(count * sizeof(float) * 4), Allocator.TempJob);
                using var vector = new NativeArray<UnityEngine.Vector4>(count, Allocator.TempJob);
                var job = new ConvertByteToVector4Job()
                {
                    ByteArray = bytes,
                    VectorArray = vector
                };
                var handle = job.Schedule(count, -1);
                handle.Complete();
                return vector.ToArray();
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

            // Burst 컴파일러를 사용한 Job 정의
            [BurstCompile]
            private struct ConvertByteToVector4Job : IJobParallelFor
            {
                [ReadOnly] public NativeArray<byte> ByteArray;
                public NativeArray<UnityEngine.Color32> ColorArray;

                public void Execute(int index)
                {
                    int byteIndex = index * 4;
                    // ByteArray에서 각 요소를 직접 변환하여 Vector3를 만듭니다.
                    UnityEngine.Color32 vector = new UnityEngine.Color32(
                        ByteArray[byteIndex],
                        ByteArray[byteIndex + 1],
                        ByteArray[byteIndex + 2],
                        ByteArray[byteIndex + 3]
                    );
                    ColorArray[index] = vector;
                }
            }

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
                using var bytes = new NativeArray<byte>(reader.ReadBytes(count * 4), Allocator.TempJob);
                using var vector = new NativeArray<UnityEngine.Color32>(count, Allocator.TempJob);
                var job = new ConvertByteToVector4Job()
                {
                    ByteArray = bytes,
                    ColorArray = vector
                };
                var handle = job.Schedule(count, -1);
                handle.Complete();
                return vector.ToArray();
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
            [BurstCompile]
            private struct ConvertByteToMatrix4x4Job : IJobParallelFor
            {
                [ReadOnly] public NativeArray<byte> ByteArray;
                public NativeArray<UnityEngine.Matrix4x4> MatrixArray;

                public void Execute(int index)
                {
                    int byteIndex = index * 16 * sizeof(float);

                    // ByteArray에서 각 요소를 직접 변환하여 Matrix4x4를 만듭니다.
                    UnityEngine.Matrix4x4 matrix = new UnityEngine.Matrix4x4
                    {
                        m00 = ReadFloatFromBytes(ByteArray, byteIndex),
                        m01 = ReadFloatFromBytes(ByteArray, byteIndex + sizeof(float)),
                        m02 = ReadFloatFromBytes(ByteArray, byteIndex + 2 * sizeof(float)),
                        m03 = ReadFloatFromBytes(ByteArray, byteIndex + 3 * sizeof(float)),
                        m10 = ReadFloatFromBytes(ByteArray, byteIndex + 4 * sizeof(float)),
                        m11 = ReadFloatFromBytes(ByteArray, byteIndex + 5 * sizeof(float)),
                        m12 = ReadFloatFromBytes(ByteArray, byteIndex + 6 * sizeof(float)),
                        m13 = ReadFloatFromBytes(ByteArray, byteIndex + 7 * sizeof(float)),
                        m20 = ReadFloatFromBytes(ByteArray, byteIndex + 8 * sizeof(float)),
                        m21 = ReadFloatFromBytes(ByteArray, byteIndex + 9 * sizeof(float)),
                        m22 = ReadFloatFromBytes(ByteArray, byteIndex + 10 * sizeof(float)),
                        m23 = ReadFloatFromBytes(ByteArray, byteIndex + 11 * sizeof(float)),
                        m30 = ReadFloatFromBytes(ByteArray, byteIndex + 12 * sizeof(float)),
                        m31 = ReadFloatFromBytes(ByteArray, byteIndex + 13 * sizeof(float)),
                        m32 = ReadFloatFromBytes(ByteArray, byteIndex + 14 * sizeof(float)),
                        m33 = ReadFloatFromBytes(ByteArray, byteIndex + 15 * sizeof(float)),
                    };

                    MatrixArray[index] = matrix;
                }
            }

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
                using var bytes = new NativeArray<byte>(reader.ReadBytes(count * 16 * sizeof(float)), Allocator.TempJob);
                using var matrix = new NativeArray<UnityEngine.Matrix4x4>(count, Allocator.TempJob);
                ConvertByteToMatrix4x4Job job = new ConvertByteToMatrix4x4Job
                {
                    ByteArray = bytes,
                    MatrixArray = matrix
                };

                JobHandle handle = job.Schedule(count, -1);
                handle.Complete();
                return matrix.ToArray();
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