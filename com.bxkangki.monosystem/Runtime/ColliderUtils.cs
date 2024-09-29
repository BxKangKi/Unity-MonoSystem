using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;
using Unity.Mathematics;

namespace MonoSystem
{
    public struct ColliderUtils
    {
        public static void NearestFromCollider(Collider[] colls, float3 origin, int count, out float3 outPos, out int index)
        {
            var position = new NativeArray<float3>(count, Allocator.TempJob);
            var distance = new NativeArray<float>(count, Allocator.TempJob);
            for (int i = 0; i < count; i++)
            {
                position[i] = colls[i].gameObject.transform.position;
            }
            var job = new DistanceJob()
            {
                position = position,
                distance = distance,
                origin = origin
            };
            JobHandle handle = job.Schedule(count, JobsConfig.InnerLoopBatchCount);
            handle.Complete();
            FindShortest(distance, out index);
            outPos = position[index];
            position.Dispose();
            distance.Dispose();
        }

        public static void NearestFromCollider(Collider[] colls, float3 origin, int count, out int index)
        {
            NearestFromCollider(colls, origin, count, out var outPos, out index);
        }

        private static void FindShortest(NativeArray<float> distance, out int index)
        {
            float shortest = distance[0];
            index = 0;
            for (int i = 0; i < distance.Length; i++)
            {
                if (shortest > distance[i])
                {
                    shortest = distance[i];
                    index = i;
                }
            }
        }


        [BurstCompile]
        public struct DistanceJob : IJobParallelFor
        {
            [WriteOnly] public NativeArray<float> distance;
            [ReadOnly] public NativeArray<float3> position;
            [ReadOnly] public float3 origin;
            public void Execute(int i)
            {
                distance[i] = math.distance(origin, position[i]);
            }
        }
    }
}