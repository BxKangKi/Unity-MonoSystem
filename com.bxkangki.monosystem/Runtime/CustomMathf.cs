using UnityEngine;
using Unity.Mathematics;

namespace MonoSystem
{
    public struct CustomMathf
    {
        public const float Rad2Deg = 180f / math.PI;
        public const float Epsilon = 0.00001f;
        private const float kEpsilonNormalSqrt = 1e-15f;

        public static quaternion LookRotation(float3 forward)
        {
            return LookRotation(forward, new float3(0f, 1f, 0f));
        }


        // https://discussions.unity.com/t/what-is-the-source-code-of-quaternion-lookrotation/72474/3
        public static quaternion LookRotation(float3 forward, float3 up)
        {
            forward = math.normalize(forward);

            float3 vector = math.normalize(forward);
            float3 vector2 = math.normalize(math.cross(up, vector));
            float3 vector3 = math.cross(vector, vector2);
            float m00 = vector2.x;
            float m01 = vector2.y;
            float m02 = vector2.z;
            float m10 = vector3.x;
            float m11 = vector3.y;
            float m12 = vector3.z;
            float m20 = vector.x;
            float m21 = vector.y;
            float m22 = vector.z;

            float num8 = (m00 + m11) + m22;
            var quaternion = new quaternion();
            if (num8 > 0f)
            {
                var num = (float)math.sqrt(num8 + 1f);
                quaternion.value.w = num * 0.5f;
                num = 0.5f / num;
                quaternion.value.x = (m12 - m21) * num;
                quaternion.value.y = (m20 - m02) * num;
                quaternion.value.z = (m01 - m10) * num;
                return quaternion;
            }
            if ((m00 >= m11) && (m00 >= m22))
            {
                var num7 = (float)math.sqrt(((1f + m00) - m11) - m22);
                var num4 = 0.5f / num7;
                quaternion.value.x = 0.5f * num7;
                quaternion.value.y = (m01 + m10) * num4;
                quaternion.value.z = (m02 + m20) * num4;
                quaternion.value.w = (m12 - m21) * num4;
                return quaternion;
            }
            if (m11 > m22)
            {
                var num6 = (float)math.sqrt(((1f + m11) - m00) - m22);
                var num3 = 0.5f / num6;
                quaternion.value.x = (m10 + m01) * num3;
                quaternion.value.y = 0.5f * num6;
                quaternion.value.z = (m21 + m12) * num3;
                quaternion.value.w = (m20 - m02) * num3;
                return quaternion;
            }
            var num5 = (float)math.sqrt(((1f + m22) - m00) - m11);
            var num2 = 0.5f / num5;
            quaternion.value.x = (m20 + m02) * num2;
            quaternion.value.y = (m21 + m12) * num2;
            quaternion.value.z = 0.5f * num5;
            quaternion.value.w = (m01 - m10) * num2;
            return quaternion;
        }

        public static float SmoothDampAngle(float current, float target, ref float currentVelocity, float smoothTime, float deltaTime, float maxSpeed = float.MaxValue)
        {
            target = current + DeltaAngle(current, target);
            return SmoothDamp(current, target, ref currentVelocity, smoothTime, deltaTime, maxSpeed);
        }

        public static float Repeat(float t, float length)
        {
            if (length == 0f)
            {
                length = 0.00001f;
            }
            return math.clamp(t - math.floor(t / length) * length, 0.0f, length);
        }

        public static float DeltaAngle(float current, float target)
        {
            float delta = Repeat((target - current), 360.0F);
            if (delta > 180.0F)
            {
                delta -= 360.0F;
            }
            return delta;
        }


        public static float Lerp(float a, float b, float t, float c)
        {
            float result = math.lerp(a, b, t);
            if (math.abs(result) < math.abs(c))
            {
                result = b;
            }
            return result;
        }


        public static float3 Lerp(float3 a, float3 b, float t, float c)
        {
            float3 result = new float3()
            {
                x = Lerp(a.x, b.x, t, c),
                y = Lerp(a.y, b.y, t, c),
                z = Lerp(a.z, b.z, t, c)
            };
            return result;
        }


        // Gradually changes a value towards a desired goal over time.
        public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime, float deltaTime, float maxSpeed = float.MaxValue)
        {
            // Based on Game Programming Gems 4 Chapter 1.10
            smoothTime = math.max(0.00001f, smoothTime);
            float omega = 2F / smoothTime;

            float x = omega * deltaTime;
            float exp = 1F / (1F + x + 0.48f * x * x + 0.235f * x * x * x);
            float change = current - target;
            float originalTo = target;

            // Clamp maximum speed
            float maxChange = maxSpeed * smoothTime;
            change = math.clamp(change, -maxChange, maxChange);
            target = current - change;

            float temp = (currentVelocity + omega * change) * deltaTime;
            currentVelocity = (currentVelocity - omega * temp) * exp;
            float output = target + (change + temp) * exp;

            // Prevent overshooting
            if (originalTo - current > 0.0f == output > originalTo)
            {
                output = originalTo;
                currentVelocity = (output - originalTo) / math.max(0.00001f, deltaTime);
            }

            return output;
        }


        public static float ClampAngle(float value, float min, float max)
        {
            if (value < -360f) value += 360f;
            if (value > 360f) value -= 360f;
            value = (value > max) ? max : ((value < min) ? min : value);
            return value;
        }

        public static float Angle(float3 from, float3 to)
        {
            // sqrt(a) * sqrt(b) = sqrt(a * b) -- valid for real numbers
            float denominator = (float)math.sqrt(SqrMagnitude(from) * SqrMagnitude(to));
            if (denominator < kEpsilonNormalSqrt)
            {
                return 0f;
            }
            float dot = math.clamp(Dot(from, to) / denominator, -1f, 1f);
            return ((float)math.acos(dot)) * Rad2Deg;
        }

        public static float Dot(float3 lhs, float3 rhs)
        {
            return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
        }

        public static float SqrMagnitude(float3 vector)
        {
            return vector.x * vector.x + vector.y * vector.y + vector.z * vector.z;
        }

        public static float Magnitude(float3 vector)
        {
            return (float)math.sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
        }

        public static float3 Project(float3 vector, float3 onNormal)
        {
            float sqrMag = Dot(onNormal, onNormal);
            if (sqrMag < Epsilon)
            {
                return new float3(0f, 0f, 0f);
            }
            else
            {
                var dot = Dot(vector, onNormal);
                return new float3(onNormal.x * dot / sqrMag,
                    onNormal.y * dot / sqrMag,
                    onNormal.z * dot / sqrMag);
            }
        }

        public static float3 Reflect(float3 inDirection, float3 inNormal)
        {
            float factor = -2F * Dot(inNormal, inDirection);
            return new float3(factor * inNormal.x + inDirection.x,
                factor * inNormal.y + inDirection.y,
                factor * inNormal.z + inDirection.z);
        }

        public static float3 Normalize(Vector3 value)
        {
            float mag = Magnitude(value);
            if (mag > Epsilon)
            {
                return value / mag;
            }
            else
            {
                return new float3(0f, 0f, 0f);
            }
        }

        public static float3 ProjectOnPlane(float3 vector, float3 planeNormal)
        {
            float sqrMag = Dot(planeNormal, planeNormal);
            if (sqrMag < Epsilon)
            {
                return vector;
            }
            else
            {
                var dot = Dot(vector, planeNormal);
                return new float3(vector.x - planeNormal.x * dot / sqrMag,
                    vector.y - planeNormal.y * dot / sqrMag,
                    vector.z - planeNormal.z * dot / sqrMag);
            }
        }

        public static float Vector3Difference(Vector3 a, Vector3 b)
        {
            return (math.abs(a.x - b.x) + math.abs(a.y - b.y) + math.abs(a.z - b.z) * 0.33333f);
        }


        public static quaternion FromToRotation(float3 fromDirection, float3 toDirection)
        {
            // 두 벡터를 정규화
            fromDirection = math.normalize(fromDirection);
            toDirection = math.normalize(toDirection);

            // 두 벡터 사이의 회전축 계산
            float3 rotationAxis = math.cross(fromDirection, toDirection);
            float sinAngle = math.length(rotationAxis);

            // 두 벡터 사이의 각도 계산
            float cosAngle = math.dot(fromDirection, toDirection);

            // 두 벡터가 거의 동일한 경우 (회전 필요 없음)
            if (sinAngle < 1e-6f)
            {
                // 두 벡터가 동일한 경우
                if (cosAngle > 0.9999f)
                    return quaternion.identity;
                // 두 벡터가 정반대인 경우
                else
                    return new quaternion(0f, 0f, 1f, 0f); // 180도 회전 (임의의 축을 기준으로)
            }

            // 쿼터니언 생성
            float angle = math.atan2(sinAngle, cosAngle); // 회전각
            float halfAngle = angle * 0.5f;
            float sinHalfAngle = math.sin(halfAngle);
            quaternion rotation = new quaternion(rotationAxis.x * sinHalfAngle, rotationAxis.y * sinHalfAngle, rotationAxis.z * sinHalfAngle, math.cos(halfAngle));
            return rotation;
        }

        public static quaternion AddRotation(quaternion a, quaternion b)
        {
            float4 lhs = a.value;
            float4 rhs = b.value;
            return new quaternion(lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y,
                                  lhs.w * rhs.y + lhs.y * rhs.w + lhs.z * rhs.x - lhs.x * rhs.z,
                                  lhs.w * rhs.z + lhs.z * rhs.w + lhs.x * rhs.y - lhs.y * rhs.x,
                                  lhs.w * rhs.w - lhs.x * rhs.x - lhs.y * rhs.y - lhs.z * rhs.z);
        }
    }
}