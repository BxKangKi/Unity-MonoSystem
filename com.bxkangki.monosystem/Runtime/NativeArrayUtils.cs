using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace MonoSystem
{
    public struct NativeArrayUtils
    {
        public static NativeArray<float> FloatArray(float value, int count)
        {
            var result = new NativeArray<float>(count, Allocator.TempJob);
            var job = new FloatArrayJob()
            {
                result = result,
                value = value
            };

            var handle = job.Schedule(count, JobsConfig.InnerLoopBatchCount);
            handle.Complete();

            return result;
        }


        public static NativeArray<float3> Float3Array(float3 value, int count)
        {
            var result = new NativeArray<float3>(count, Allocator.TempJob);
            var job = new Float3ArrayJob()
            {
                result = result,
                value = value
            };

            var handle = job.Schedule(count, JobsConfig.InnerLoopBatchCount);
            handle.Complete();

            return result;
        }
    }

    [BurstCompile]
    public struct FloatArrayJob : IJobParallelFor
    {
        [WriteOnly] public NativeArray<float> result;
        [ReadOnly] public float value;
        public void Execute(int i)
        {
            result[i] = value;
        }
    }

    [BurstCompile]
    public struct Float3ArrayJob : IJobParallelFor
    {
        [WriteOnly] public NativeArray<float3> result;
        [ReadOnly] public float3 value;
        public void Execute(int i)
        {
            result[i] = value;
        }
    }
}