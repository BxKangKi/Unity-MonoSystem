using System;
using System.Security.Cryptography;

// https://moondongjun.tistory.com/96

namespace FileSystem
{
    public struct HashSystem
    {
        /// <summary>
        /// Generate GUID string from MD5 hash from string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetGUID(int integer)
        {
            byte[] hashBytes;
            using (MD5 md5Hash = MD5.Create())
            {
                hashBytes = md5Hash.ComputeHash(BitConverter.GetBytes(integer));
            }
            return new Guid(hashBytes).ToString();
        }

        /// <summary>
        /// Generate GUID string from MD5 hash from string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetGUID(string str)
        {
            byte[] hashBytes;
            using (MD5 md5Hash = MD5.Create())
            {
                hashBytes = md5Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str));
            }
            return new Guid(hashBytes).ToString();
        }


        /// <summary>
        /// Generate GUID string from MD5 hash from bytes
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetGUID(byte[] bytes)
        {
            byte[] hashBytes;
            using (MD5 md5Hash = MD5.Create())
            {
                hashBytes = md5Hash.ComputeHash(bytes);
            }
            return new Guid(hashBytes).ToString();
        }


        public static string GetGUID()
        {
            return System.Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Generate SHA256 hash string from string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetFixedHashCode(string str)
        {
            byte[] hashBytes;
            using (SHA256 sha256Hash = SHA256.Create())
            {
                hashBytes = sha256Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str));
            }
            return BitConverter.ToString(hashBytes).Replace("-", "");
        }

        /// <summary>
        /// Generate SHA256 hash string from bytes
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetFixedHashCode(byte[] bytes)
        {
            byte[] hashBytes;
            using (SHA256 sha256Hash = SHA256.Create())
            {
                hashBytes = sha256Hash.ComputeHash(bytes);
            }
            return BitConverter.ToString(hashBytes).Replace("-", "");
        }
    }
}