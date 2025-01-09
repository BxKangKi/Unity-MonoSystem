using System;

namespace FileSystem
{
    public struct ArrayUtility
    {
        /// <summary>
        /// Add a value of T to input T[]. This process convert array to List<T>() and reconvert to T[].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T[] Add<T>(T[] input, T value)
        {
            int count = input.Length;
            var result = new T[count + 1];
            Array.Copy(input, result, count);
            result[count] = value;
            return result;
        }

        /// <summary>
        /// Remove a value of T from input T[]. This process convert array to List<T>() and reconvert to T[].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T[] Remove<T>(T[] input, T value)
        {
            int count = input.Length - 1;
            var result = new T[count];
            int index = Array.IndexOf(input, value);
            Array.Copy(input, 0, result, 0, index);
            Array.Copy(input, index + 1, result, index + 1, count - index);
            return result;
        }
    }
}
