using System.Collections.Generic;

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
            var list = new List<T>();
            list.AddRange(input);
            list.Add(value);
            return list.ToArray();
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
            var list = new List<T>();
            list.AddRange(input);
            if (list.Contains(value))
            {
                list.Remove(value);
            }
            return list.ToArray();
        }
    }
}
